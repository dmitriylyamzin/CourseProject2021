using System;
using System.IO;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Collections.Generic;
using VkNet.Enums.SafetyEnums;

namespace SmartFriendShipper
{
    class Program
    {
        // Статические поля хранящие объекты нейросети, сборщика данных, сервисов ВКонтакте и двух объектов для подключения е API.
        private static FriendShipperNeuralNetwork fs_network = new FriendShipperNeuralNetwork();
        private static DataCollector dc = new DataCollector();
        private static ServiceCollection services = new ServiceCollection();
        private static VkApi api = new VkApi(services);
        private static VkApi vk_api = new VkApi(services);

        // Вложенный класс, позволяющий реализовать многопоточную работу чат-бота.
        private class ThreadHelper
        {
            // Защищённые поля - строка входного сообщения и идентификатор пользователя, который его отправил
            protected string user_message;
            protected long user_id;

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            /// <param name="user_message">Строка входного сообщения.</param>
            /// <param name="user_id">Идентификатор отправителя.</param>
            public ThreadHelper(string user_message, long user_id)
            {
                this.user_message = user_message;
                this.user_id = user_id;
            }

            /// <summary>
            /// Метод, отвечающий за отправку сообщения пользователю.
            /// </summary>
            /// <param name="message">Текст сообщения.</param>
            /// <param name="userID">Идентификатор адресата.</param>
            public void SendMessage(string message, long? userID)
            {
                Random rnd = new Random();
                try
                {
                    vk_api.Messages.Send(new MessagesSendParams
                    {
                        RandomId = rnd.Next(),
                        UserId = userID,
                        Message = message
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            /// <summary>
            /// Метод, обрабатывающий профиль пользователя для получения открытой информации о нём.
            /// </summary>
            /// <param name="user_id">Идентификатор пользователя.</param>
            /// <returns>Объект класса User с проинициализированными полями.</returns>
            public User GetUserData(long user_id)
            {
                try
                {
                    Thread.Sleep(500);
                    var current_user = api.Users.Get(new long[] { user_id }, ProfileFields.All).FirstOrDefault();
                    Thread.Sleep(500);
                    User user_obj = new User(current_user.Id)
                    {
                        UserName = current_user.FirstName,
                        UserSurname = current_user.LastName,
                        UserBdate = current_user.BirthDate,
                        UserAbout = current_user.About,
                        UserActivities = current_user.Activities,
                        UserBooks = current_user.Books,
                        UserCareer = current_user.Career,
                        UserCity = current_user.City,
                        UserContacts = current_user.Contacts,
                        UserCounters = current_user.Counters,
                        UserCountry = current_user.Country,
                        UserDomain = current_user.Domain,
                        UserEducation = current_user.Education,
                        UserExports = current_user.Exports,
                        UserAge = -1
                    };
                    if (current_user.FollowersCount > 0)
                    {
                        user_obj.UserFollowersCount = current_user.FollowersCount;
                    }
                    else
                    {
                        user_obj.UserFollowersCount = 0;
                    }
                    user_obj.UserGames = current_user.Games;
                    user_obj.UserHomePhone = current_user.HomePhone;
                    user_obj.UserHomeTown = current_user.HomeTown;
                    user_obj.UserInterests = current_user.Interests;
                    user_obj.UserIsClosed = current_user.IsClosed;
                    user_obj.UserIsDeactivated = current_user.IsDeactivated;
                    user_obj.UserLastSeen = current_user.LastSeen;
                    user_obj.UserMilitary = current_user.Military;
                    user_obj.UserMovies = current_user.Movies;
                    user_obj.UserMusic = current_user.Music;
                    user_obj.UserOccupation = current_user.Occupation;
                    user_obj.UserQuotes = current_user.Quotes;
                    user_obj.UserRelation = current_user.Relation;
                    user_obj.UserRelatives = current_user.Relatives;
                    user_obj.UserSchools = current_user.Schools;
                    user_obj.UserSite = current_user.Site;
                    user_obj.UserStandInLife = current_user.StandInLife;
                    user_obj.UserTv = current_user.Tv;
                    user_obj.UserUniversities = current_user.Universities;
                    return user_obj;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            /// <summary>
            /// Метод, определяющий целочисленный идентификатор пользователя по короткому адресу его страницы.
            /// </summary>
            /// <param name="string_id">Короткий адрес страницы.</param>
            /// <returns>Целочисленный идентияикатор.</returns>
            public long FindId(string string_id)
            {
                bool id_flag = false;
                // Удаляем начала ссылки, оставляя только короткий адрес страницы.
                if (string_id.StartsWith("https://m.vk.com/") || string_id.StartsWith("https://vk.com/"))
                {
                    string_id = string_id.Replace("https://m.vk.com/", "");
                    string_id = string_id.Replace("https://vk.com/", "");
                }
                // Удаляем префикс "id", чтобы получить целочисленный идентификатор пользователя. 
                //Если всё-таки был задан короткий адрес, а не число, истинное значение флага приведёт к тому, что
                // значение адреса будет восстановлено.
                if (string_id.StartsWith("id"))
                {
                    string_id = string_id.Substring(2);
                    id_flag = true;
                }
                if (!long.TryParse(string_id, out long real_id))
                {
                    if (id_flag == true)
                    {
                        string_id = "id" + string_id;
                    }

                    // Попытка распознать идентификатор пользователя, зная короткий адрес его страницы.
                    // В случае неудачи возвращается 0, что будет означать, что адрес указан неверно.
                    VkObject obj = null;
                    try
                    {
                        obj = vk_api.Utils.ResolveScreenName(string_id);
                    }
                    catch (Exception)
                    {
                    }
                    if (obj != null)
                    {
                        real_id = (long)obj.Id;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return real_id;
            }

            /// <summary>
            /// Метод, распознающий запрос пользователя.
            /// </summary>
            /// <param name="message">Сообщения пользователя.</param>
            /// <param name="user_id">Идентификатор отправителя.</param>
            /// <param name="mode_flag">Запрашиваемый режим работы.</param>
            /// <returns>Список пар идентификаторов пользователей, вероятность начала дружбы между которыми необходимо проверить.</returns>
            public List<List<long>> UnderstandUserMessage(string message, long user_id, ref int mode_flag)
            {
                List<List<long>> pairs = new List<List<long>>();
                if (message.StartsWith("!"))
                {
                    mode_flag = 1;
                    message = message.Substring(1);
                }
                if (message.StartsWith("?"))
                {
                    mode_flag = 2;
                    message = message.Substring(1);
                }
                string[] tasks = message.Split(',');
                foreach (string unchecked_task in tasks)
                {
                    string task = unchecked_task.Trim(' ');
                    string[] id_list = task.Split(' ');
                    if (id_list.Length > 2 || id_list.Length < 1)
                    {
                        return null;
                    }
                    else
                    {
                        List<long> pair = new List<long>();
                        if (id_list.Length == 1)
                        {
                            pair.Add(user_id);
                            pair.Add(FindId(id_list[0]));
                            pairs.Add(pair);
                        }
                        else
                        {
                            pair.Add(FindId(id_list[0]));
                            pair.Add(FindId(id_list[1]));
                            pairs.Add(pair);
                        }
                    }
                }
                return pairs;
            }

            /// <summary>
            /// Метод, возвращающий объекты пользователей из входного запроса.
            /// </summary>
            /// <param name="pairs">Список пар идентификаторов пользователей из входного запроса.</param>
            /// <param name="user_id">Идентификатор текущего пользователя.</param>
            /// <param name="user_obj">Объект текущего пользователя.</param>
            /// <returns>Список пар объектов пользователей, вероятность начала дружбы между которыми надо вычислить.</returns>
            public List<List<User>> GetUserObjects(List<List<long>> pairs, long user_id, User user_obj)
            {
                List<List<User>> user_objects_list = new List<List<User>>();
                foreach (List<long> pair in pairs)
                {
                    long id1 = pair[0];
                    long id2 = pair[1];
                    try
                    {
                        User user1_obj;
                        if (id1 == user_id)
                        {
                            user1_obj = user_obj;
                        }
                        else
                        {
                            user1_obj = GetUserData(id1);
                        }
                        User user2_obj = GetUserData(id2);
                        if (user1_obj != null && user2_obj != null)
                        {
                            List<User> objects_pair = new List<User>
                            {
                            user1_obj,
                            user2_obj
                            };
                            user_objects_list.Add(objects_pair);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                }
                return user_objects_list;
            }

            /// <summary>
            /// Метод, получающий список друзей пользователя.
            /// </summary>
            /// <param name="user">Объект пользователя.</param>
            /// <returns>Список объектов друзей пользователя.</returns>
            public List<VkNet.Model.User> GetFriendsList(User user)
            {
                var user_friend_list = api.Friends.Get(new FriendsGetParams
                {
                    UserId = user.UserId,
                    Count = 5000,
                    Fields = ProfileFields.All
                }).ToList();

                return user_friend_list;
            }

            /// <summary>
            /// Метод, превращающий данные о двух пользователях во входные данные для нейросети.
            /// </summary>
            /// <param name="data">Строка с данными, собранными для двух пользователей.</param>
            /// <returns>Список чисел от 0 до 1 для входного слоя нейросети.</returns>
            public List<double> NormalizeData(string data)
            {
                List<double> new_data = new List<double>();
                string[] data_array = data.Split(' ');
                for (int i = 0; i < data_array.Length; i++)
                {
                    if (i != 0 && i != 8)
                    {
                        new_data.Add(fs_network.ActivationFunction(long.Parse(data_array[i])));
                    }
                }
                return new_data;
            }

            /// <summary>
            /// Метод, вычисляющий вероятность начала дружбы пользователя с его реальными друзьями.
            /// </summary>
            /// <param name="user_obj">Объект пользователя.</param>
            /// <param name="friend_list">Список друзей пользователя.</param>
            /// <param name="elite_id">Значение идентификатора самого близкого друга пользователя.</param>
            /// <param name="elite_change">Флаг, опиывающий необходимость нахождения самого близкого друга пользователя.</param>
            /// <param name="elite_probability">Вероятность начала дружбы пользователя с его самым близким другом</param>
            /// <returns>Средняя вероятность начала дружбы пользователя с его реальными друзьями.</returns>
            public double CountFriendsProbability(User user_obj, List<VkNet.Model.User> friend_list, ref long elite_id, bool elite_change, ref double elite_probability)
            {
                double max_probability = 0;
                double total_friends_probability = 0;
                int count_of_friends = 0;
                foreach (VkNet.Model.User friend in friend_list)
                {
                    var friend_obj = GetUserData(friend.Id);
                    string friend_data = dc.GetInputData(api, user_obj, friend_obj);
                    if (friend_data != null)
                    {
                        double current_probability = fs_network.DeepFeedForward(NormalizeData(friend_data));
                        total_friends_probability += current_probability;
                        if (elite_change)
                        {

                            if (max_probability < current_probability)
                            {
                                max_probability = current_probability;
                                elite_id = friend_obj.UserId;
                                elite_probability = max_probability;
                            }
                        }
                        count_of_friends += 1;
                    }
                }
                if (count_of_friends == 0)
                {
                    return 1;
                }
                return total_friends_probability / count_of_friends;
            }

            /// <summary>
            /// Метод, вычисляющий вероятность начала дружбы пользователя с его реальными подписчиками.
            /// </summary>
            /// <param name="user_obj">Объект пользователя.</param>
            /// <param name="follower_list">Список подписчиков пользователя.</param>
            /// <returns>Средняя вероятность начала дружбы пользователя с его реальными подписчиками.</returns>
            public double CountFollowersProbability(User user_obj, VkNet.Utils.VkCollection<VkNet.Model.User> follower_list)
            {
                double total_followers_probability = 0;
                int count_of_followers = 0;
                foreach (VkNet.Model.User friend in follower_list)
                {
                    string follower_data = dc.GetInputData(api, user_obj, GetUserData(friend.Id));
                    if (follower_data != null)
                    {
                        total_followers_probability += fs_network.DeepFeedForward(NormalizeData(follower_data));
                        Console.WriteLine(follower_data);
                        count_of_followers += 1;
                    }
                }
                if (count_of_followers == 0)
                {
                    return 1;
                }
                return total_followers_probability / count_of_followers;
            }

            /// <summary>
            /// Метод, обрабатывающий запросы пользователя в углублённом или элитном режиме.
            /// </summary>
            /// <param name="user_obj1">Объект первого пользователя из пары идентификаторов.</param>
            /// <param name="user_obj2">Объект второго пользователя из пары идентификаторов.</param>
            /// <param name="first_user_probability">Вероятность начала дружбы межды первым и вторым пользователем в обычном режиме.</param>
            /// <param name="second_user_probability">Вероятность начала дружбы межды вторым и первым пользователем в обычном режиме.</param>
            /// <param name="elite_flag">Флаг, выражающий необходимость обработки запросов в элитном режиме.</param>
            /// <param name="elite_id">Иденификатор пользователя, считающегося самым близким другом выбранного пользователя.</param>
            /// <param name="elite_probability">Вероятность начала дружбы между пользователем и его самым близким другом.</param>
            /// <returns>Вероятность начала дружбы между первым и вторым пользователем.</returns>
            public double ProcessDeepModes(User user_obj1, User user_obj2, double first_user_probability, double second_user_probability, bool elite_flag, ref long elite_id, ref double elite_probability)
            {
                List<VkNet.Model.User> friend_list1;
                List<VkNet.Model.User> friend_list2;
                VkNet.Utils.VkCollection<VkNet.Model.User> follower_list1;
                VkNet.Utils.VkCollection<VkNet.Model.User> follower_list2;
                try
                {
                    friend_list1 = GetFriendsList(user_obj1);
                    friend_list2 = GetFriendsList(user_obj2);
                    follower_list1 = api.Users.GetFollowers(user_obj1.UserId, 1000, 0, ProfileFields.All);
                    follower_list2 = api.Users.GetFollowers(user_obj2.UserId, 1000, 0, ProfileFields.All);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not check friends and followers");
                    return (first_user_probability + second_user_probability) / 2;
                }
                double friends_front1;
                if (!elite_flag)
                {
                    friends_front1 = CountFriendsProbability(user_obj1, friend_list1, ref elite_id, false, ref elite_probability);
                }
                else
                {
                    friends_front1 = CountFriendsProbability(user_obj1, friend_list1, ref elite_id, true, ref elite_probability);
                }
                double friends_front2 = CountFriendsProbability(user_obj2, friend_list2, ref elite_id, false, ref elite_probability);
                double followers_front1 = CountFollowersProbability(user_obj1, follower_list1);
                double followers_front2 = CountFollowersProbability(user_obj2, follower_list2);
                first_user_probability = Math.Abs(first_user_probability - followers_front1) / (Math.Abs(first_user_probability - followers_front1) + Math.Abs(first_user_probability - friends_front1));
                second_user_probability = Math.Abs(second_user_probability - followers_front2) / (Math.Abs(second_user_probability - followers_front2) + Math.Abs(second_user_probability - friends_front2));
                Console.WriteLine(first_user_probability.ToString() + " " + second_user_probability.ToString());
                return (first_user_probability + second_user_probability) / 2;
            }

            /// <summary>
            /// Метод, обрабатывающий запрос текущего пользователя.
            /// </summary>
            /// <param name="objects_list">Список пар объектов пользователей, вероятность дружбы между которыми необходимо вычислить.</param>
            /// <param name="mode_flag">Значение режима работы (0  обычный, 1 - углублённый, 2 - элитный).</param>
            public void ProcessRequests(List<List<User>> objects_list, int mode_flag)
            {
                string result = "";
                long elite_id = 0;
                double elite_probability = 0;
                foreach (List<User> users_pair in objects_list)
                {
                    double final_probability = 0;
                    User user_obj1 = users_pair[0];
                    User user_obj2 = users_pair[1];
                    if (user_obj1.UserId == user_obj2.UserId)
                    {
                        final_probability = 1;
                    }
                    else
                    {
                        string data = dc.GetInputData(api, user_obj1, user_obj2);
                        string reversed_data = dc.GetInputData(api, user_obj2, user_obj1);
                        if (data == null || reversed_data == null)
                        {
                            SendMessage("Кажется, вы указали в запросе несуществующего пользователя или пользователя с закрытым профилем. Исправьте, пожалуйста, ошибку и повторите попытку", user_id);
                            continue;
                        }
                        Console.WriteLine(data);
                        Console.WriteLine(reversed_data);
                        double first_user_probability = fs_network.DeepFeedForward(NormalizeData(data));
                        double second_user_probability = fs_network.DeepFeedForward(NormalizeData(reversed_data));
                        if (mode_flag == 0)
                        {
                            final_probability = (first_user_probability + second_user_probability) / 2;
                        }
                        if (mode_flag == 1)
                        {
                            final_probability = ProcessDeepModes(user_obj1, user_obj2, first_user_probability, second_user_probability, false, ref elite_id, ref elite_probability);
                        }
                        if (mode_flag == 2)
                        {
                            final_probability = ProcessDeepModes(user_obj1, user_obj2, first_user_probability, second_user_probability, true, ref elite_id, ref elite_probability);
                        }
                    }
                    string name1 = user_obj1.UserSurname + " " + user_obj1.UserName;
                    string name2 = user_obj2.UserSurname + " " + user_obj2.UserName;
                    result += "Вероятность того, что пользователь " + name1 + " сможет дружить с пользователем " + name2 + ":\n";
                    result += (final_probability * 100).ToString() + "%\n";
                }
                if (mode_flag == 2 && elite_id != 0)
                {
                    User user = GetUserData(elite_id);
                    while (user == null)
                    {
                        Thread.Sleep(500);
                        user = GetUserData(elite_id);
                    }
                    string name = user.UserSurname + " " + user.UserName;
                    result += $"По моему скромному мнению лучший друг выбранного вами пользователя - {name}.\nВероятность начала дружбы с ним:\n{elite_probability}%\n";
                }
                SendMessage(result, user_id);
            }

            /// <summary>
            /// Метод, реализующий формирование и отправку входных данных программы.
            /// </summary>
            public void MakeAnswer()
            {
                int mode_flag = 0;
                User user_obj;
                try
                {
                    user_obj = GetUserData(user_id);
                }
                catch (Exception)
                {
                    SendMessage("К сожалению, вы не можете воспользоваться функциями данного бота до тех пор, пока ваш профиль скрыт от меня настройками приватности.", user_id);
                    return;
                }
                if (user_obj == null)
                {
                    SendMessage("К сожалению, вы не можете воспользоваться функциями данного бота до тех пор, пока ваш профиль скрыт от меня настройками приватности.", user_id);
                    return;
                }
                List<List<long>> pairs = UnderstandUserMessage(user_message, user_id, ref mode_flag);
                if (pairs == null)
                {
                    SendMessage("Кажется, запрос был сделан с ошибками, проверьте его на соответствие требованиям, указанным в самом первом сообщении диалога и повторите попытку.", user_id);
                }
                else
                {
                    List<List<User>> objects_list = GetUserObjects(pairs, user_id, user_obj);
                    if (objects_list == null)
                    {
                        SendMessage("Кажется, вы указали в запросе несуществующего пользователя или пользователя с закрытым профилем. Исправьте, пожалуйста, ошибку и повторите попытку", user_id);
                    }
                    else
                    {
                        ProcessRequests(objects_list, mode_flag);
                    }
                }
            }

        }

        /// <summary>
        /// Метод авторизации для получения доступа к API со стороны пользователя и со стороны сообщества.
        /// </summary>
        public static void Authorize()
        {
            string[] file_lines = { " ", " " };
            try
            {
                file_lines = File.ReadAllLines("RegistrationFile.txt");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message + " Некорректное имя файла!");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message + " Файла с таким именем не существует!");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message + " Некорректный результат взаимодействия с файлом!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Произошла ошибка во время работы с файлом!");
            }

            string my_login = file_lines[0];
            string my_password = file_lines[1];
            string my_key = file_lines[2];
            int autorization_flag = 0;
            while (autorization_flag != 1)
            {
                try
                {
                    vk_api.Authorize(new ApiAuthParams
                    {
                        AccessToken = my_key
                    });
                    autorization_flag = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            autorization_flag = 0;
            while (autorization_flag != 1)
            {
                try
                {
                    api.Authorize(new ApiAuthParams
                    {
                        ApplicationId = 7326944,
                        Login = my_login,
                        Password = my_password,
                        Settings = Settings.All
                    });
                    autorization_flag = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }


        static void Main()
        {
            Console.WriteLine(DateTime.Now);
            Authorize();
            // Цикл для отслеживания состояния сервера.
            while (true)
            {
                var collection = vk_api.Groups.GetLongPollServer(204524092);
                Console.WriteLine(collection.Server + ' ' + collection.Ts + ' ' + collection.Key);
                var poll = vk_api.Groups.GetBotsLongPollHistory(
                new BotsLongPollHistoryParams()
                { Server = collection.Server, Ts = collection.Ts, Key = collection.Key, Wait = 25 });
                if (poll?.Updates == null)
                {
                    Console.WriteLine("Новых сообщений нет");
                    continue;
                }
                foreach (var a in poll.Updates)
                {
                    if (a.Type == GroupUpdateType.MessageNew)
                    {
                        string user_message = a.Message.Text.ToLower();
                        long? user_id = a.Message.PeerId;

                        // Запуск нового потока для каждого нового сообщения.
                        ThreadHelper th = new ThreadHelper(user_message, (long)user_id);
                        Thread new_user_iteration = new Thread(new ThreadStart(th.MakeAnswer));
                        new_user_iteration.Start();
                        Console.WriteLine("Работа потока завершена");
                    }
                    else
                    {
                        Console.WriteLine("Нет новых сообщений");
                    }
                }
                Console.WriteLine(DateTime.Now);
            }
        }
    }
}
