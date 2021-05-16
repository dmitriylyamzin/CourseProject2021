using System;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using System.Collections.Generic;
using System.Threading;

namespace SmartFriendShipper
{
    class DataCollector
    {

        /// <summary>
        /// Метод, переводящий строковые значения полей жизненрой позиции в целые числа.
        /// </summary>
        /// <param name="words">Строковое значение параметра.</param>
        /// <returns>Числовое значение параметра.</returns>
        public static int ConvertWordsToInt(string words)
        {
            if (words == "MindAndCreativity" || words == "NotMarried" || words == "FamilyAndChildren" || words == "Communist" || words == "VeryNegative")
            {
                return 1;
            }
            if (words == "Socialist" || words == "KindnessAndHonesty" || words == "CareerAndMoney" || words == "HasFriend" || words == "Negative")
            {
                return 2;
            }
            if (words == "HealthAndBeauty" || words == "Moderate" || words == "Activities" || words == "Engaged" || words == "Compromice")
            {
                return 3;
            }
            if (words == "PowerAndWealth" || words == "Liberal" || words == "ScienceAndResearch" || words == "Married" || words == "Neutral")
            {
                return 4;
            }
            if (words == "Conservative" || words == "ImprovingTheWorld" || words == "CourageAndPersistence" || words == "ItsComplex" || words == "Positive")
            {
                return 5;
            }
            if (words == "SelfDevelopment" || words == "HumorAndLoveForLife" || words == "Monarchist" || words == "InActiveSearch")
            {
                return 6;
            }
            if (words == "BeautyAndArt" || words == "Ultraconservative" || words == "Amorous")
            {
                return 7;
            }
            if (words == "FameAndInfluence" || words == "Apathetic" || words == "CivilMarriage")
            {
                return 8;
            }
            if (words == "Libertarian")
            {
                return 9;
            }
            return 0;
        }

        /// <summary>
        /// Метод, позволяющий получить параметры, которые отвечают за дополнительную совместимость между двумя пользователями.
        /// </summary>
        /// <param name="api">Экземпляр объекта для доступа к API.</param>
        /// <param name="user_obj">Объект первого пользователя из пары.</param>
        /// <param name="priend_obj">Объект второго пользователя из пары.</param>
        /// <returns>Строка с целочисленными данными, разделёнными пробелами.</returns>
        public static string CountAdditionalParameters(VkApi api, User user_obj, User priend_obj)
        {
            string result = "";
            int count_of_common_relatives = user_obj.UserRelatives.Intersect(priend_obj.UserRelatives).Count();
            result += count_of_common_relatives.ToString() + " ";
            int count_of_common_friends = 0;
            try
            {
                var common_friends_ids = api.Friends.GetMutual(new VkNet.Model.RequestParams.FriendsGetMutualParams
                {
                    TargetUid = user_obj.UserId,
                    SourceUid = priend_obj.UserId
                });
                count_of_common_friends = common_friends_ids.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            result += count_of_common_friends.ToString() + " ";

            int common_count = 0;
            if (user_obj.UserAbout != null && priend_obj.UserAbout != null)
            {
                string[] user_about = user_obj.UserAbout.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_about = priend_obj.UserAbout.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_about.Length; i++)
                {
                    for (int j = 0; j < priend_about.Length; j++)
                    {
                        if (user_about[i] == priend_about[j] && user_about[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_about.Length + priend_about.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_about.Length + priend_about.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 ";
            }

            if (user_obj.UserBooks != null && priend_obj.UserBooks != null)
            {
                string[] user_books = user_obj.UserBooks.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_books = priend_obj.UserBooks.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_books.Length; i++)
                {
                    for (int j = 0; j < priend_books.Length; j++)
                    {
                        if (user_books[i] == priend_books[j] && user_books[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_books.Length + priend_books.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_books.Length + priend_books.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 ";
            }

            if (user_obj.UserTv != null && priend_obj.UserTv != null)
            {
                string[] user_tv = user_obj.UserTv.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_tv = priend_obj.UserTv.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_tv.Length; i++)
                {
                    for (int j = 0; j < priend_tv.Length; j++)
                    {
                        if (user_tv[i] == priend_tv[j] && user_tv[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_tv.Length + priend_tv.Length >= 2)
                {
                    result += common_count.ToString() + " " + (((user_tv.Length + priend_tv.Length) / 2).ToString()) + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 ";
            }

            if (user_obj.UserGames != null && priend_obj.UserGames != null)
            {
                string[] user_games = user_obj.UserGames.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_games = priend_obj.UserGames.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_games.Length; i++)
                {
                    for (int j = 0; j < priend_games.Length; j++)
                    {
                        if (user_games[i] == priend_games[j] && user_games[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_games.Length + priend_games.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_games.Length + priend_games.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;

                string[] user_interests = user_obj.UserInterests.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_interests = priend_obj.UserInterests.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_interests.Length; i++)
                {
                    for (int j = 0; j < priend_interests.Length; j++)
                    {
                        if (user_interests[i] == priend_interests[j] && user_interests[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_interests.Length + priend_interests.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_interests.Length + priend_interests.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 0 0 ";
            }

            if (user_obj.UserMovies != null && priend_obj.UserMovies != null)
            {
                string[] user_movies = user_obj.UserMovies.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_movies = priend_obj.UserMovies.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_movies.Length; i++)
                {
                    for (int j = 0; j < priend_movies.Length; j++)
                    {
                        if (user_movies[i] == priend_movies[j] && user_movies[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_movies.Length + priend_movies.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_movies.Length + priend_movies.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 ";
            }

            if (user_obj.UserMusic != null && priend_obj.UserMusic != null)
            {
                string[] user_music = user_obj.UserMusic.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_music = priend_obj.UserMusic.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_music.Length; i++)
                {
                    for (int j = 0; j < priend_music.Length; j++)
                    {
                        if (user_music[i] == priend_music[j] && user_music[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_music.Length + priend_music.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_music.Length + priend_music.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 ";
            }

            if (user_obj.UserQuotes != null && priend_obj.UserQuotes != null)
            {

                string[] user_quotes = user_obj.UserQuotes.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_quotes = priend_obj.UserQuotes.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                for (int i = 0; i < user_quotes.Length; i++)
                {
                    for (int j = 0; j < priend_quotes.Length; j++)
                    {
                        if (user_quotes[i] == priend_quotes[j] && user_quotes[i].Length > 2)
                        {
                            common_count += 1;
                        }
                    }
                }
                if (user_quotes.Length + priend_quotes.Length >= 2)
                {
                    result += common_count.ToString() + " " + ((user_quotes.Length + priend_quotes.Length) / 2).ToString() + " ";
                }
                else
                {
                    result += "0 0 ";
                }
                common_count = 0;
            }
            else
            {
                result += "0 0 ";
            }

            if (user_obj.UserStandInLife != null && priend_obj.UserStandInLife != null)
            {
                if (user_obj.UserStandInLife.Religion != null && priend_obj.UserStandInLife.Religion != null)
                {
                    string[] user_religion = user_obj.UserStandInLife.Religion.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                    string[] priend_religion = priend_obj.UserStandInLife.Religion.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                    for (int i = 0; i < user_religion.Length; i++)
                    {
                        for (int j = 0; j < priend_religion.Length; j++)
                        {
                            if (user_religion[i] == priend_religion[j] && user_religion[i].Length > 2)
                            {
                                common_count += 1;
                            }
                        }
                    }
                    if (user_religion.Length + priend_religion.Length >= 2)
                    {
                        result += common_count.ToString() + " " + ((user_religion.Length + priend_religion.Length) / 2).ToString() + " ";
                    }
                    else
                    {
                        result += "0 0 ";
                    }
                    common_count = 0;
                }
                else
                {
                    result += "0 0 ";
                }

                if (user_obj.UserStandInLife.InspiredBy != null && priend_obj.UserStandInLife.InspiredBy != null)
                {
                    string[] user_inspiredBy = user_obj.UserStandInLife.InspiredBy.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                    string[] priend_inspiredBy = priend_obj.UserStandInLife.InspiredBy.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                    for (int i = 0; i < user_inspiredBy.Length; i++)
                    {
                        for (int j = 0; j < priend_inspiredBy.Length; j++)
                        {
                            if (user_inspiredBy[i] == priend_inspiredBy[j] && user_inspiredBy[i].Length > 2)
                            {
                                common_count += 1;
                            }
                        }
                    }
                    if (user_inspiredBy.Length + priend_inspiredBy.Length >= 2)
                    {
                        result += common_count.ToString() + " " + ((user_inspiredBy.Length + priend_inspiredBy.Length) / 2).ToString() + " ";
                    }
                    else
                    {
                        result += "0 0 ";
                    }
                }
                else
                {
                    result += "0 0 ";
                }

                int count_of_common_languages = 0;
                if (user_obj.UserStandInLife.Languages != null && priend_obj.UserStandInLife.Languages != null)
                {
                    var user_languages = user_obj.UserStandInLife.Languages;
                    var priend_languages = priend_obj.UserStandInLife.Languages;
                    for (int i = 0; i < user_languages.Count; i++)
                    {
                        for (int j = 0; j < priend_languages.Count; j++)
                        {
                            if (user_languages[i] == priend_languages[j] && user_languages[i].ToLower() != "russian" && user_languages[i].ToLower() != "русский")
                            {
                                count_of_common_languages += 1;
                            }
                        }
                    }
                }

                result += count_of_common_languages.ToString() + " ";

            }
            else
            {
                result += "0 0 0 0 0 ";
            }
            int sites = 0;
            if (user_obj.UserSite != null && priend_obj.UserSite != null)
            {
                if (user_obj.UserSite.Length > 0 && priend_obj.UserSite.Length > 0)
                {
                    sites = 1;
                }
            }
            result += sites.ToString() + " ";
            int common_city = 0;
            if (user_obj.UserHomeTown != null && priend_obj.UserHomeTown != null)
            {
                if (user_obj.UserHomeTown.Length > 0 && user_obj.UserHomeTown == priend_obj.UserHomeTown.ToLower())
                {
                    common_city += 1;
                }
            }
            result += common_city.ToString() + " ";
            common_city = 0;
            if (user_obj.UserCity != null && priend_obj.UserCity != null)
            {
                if (user_obj.UserCity.Title.Length > 0 && user_obj.UserCity.Title == priend_obj.UserCity.Title.ToLower())
                {
                    common_city += 1;
                }
            }

            result += common_city.ToString() + " ";

            int common_country = 0;
            if (user_obj.UserCountry != null && priend_obj.UserCountry != null)
            {
                if (user_obj.UserCountry.Title.Length > 0 && user_obj.UserCountry.Title.ToLower() == priend_obj.UserCountry.Title.ToLower())
                {
                    common_country = 1;
                }
            }
            result += common_country.ToString() + " ";
            int common_military_years = 0;
            int common_military_ids = 0;
            if (user_obj.UserMilitary != null && priend_obj.UserMilitary != null)
            {
                if (user_obj.UserMilitary.From == priend_obj.UserMilitary.From && user_obj.UserMilitary.Until == priend_obj.UserMilitary.Until && priend_obj.UserMilitary.Until != 0)
                {
                    common_military_years = 1;
                }
                else
                {
                    if ((user_obj.UserMilitary.From == priend_obj.UserMilitary.From || user_obj.UserMilitary.Until == priend_obj.UserMilitary.Until) && priend_obj.UserMilitary.Until != 0)
                    {
                        common_military_years = 1;
                    }
                }

                if (user_obj.UserMilitary.UnitId == priend_obj.UserMilitary.UnitId && priend_obj.UserMilitary.UnitId != 0)
                {
                    common_military_ids = 1;
                }
            }

            result += common_military_years.ToString() + " " + common_military_ids.ToString() + " ";

            int common_career = 0;
            try
            {
                for (int i = 0; i < priend_obj.UserCareer.Count; i++)
                {
                    if (user_obj.UserCareer != null && user_obj.UserCareer[0].Position != null && (user_obj.UserCareer[0].Position.ToLower() == priend_obj.UserCareer[i].Position.ToLower() || user_obj.UserCareer[0].Company.ToLower() == priend_obj.UserCareer[i].Company.ToLower()))
                    {
                        common_career += 1;
                    }
                }

            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Данных о карьере одного из пользователей не обнаружено");
            }
            catch (Exception)
            {
                Console.WriteLine("Данных о карьере одного из пользователей не обнаружено");
            }

            result += common_career.ToString() + " ";

            int common_education = 0;
            if (user_obj.UserEducation != null && priend_obj.UserEducation != null)
            {
                if (user_obj.UserEducation.UniversityName == priend_obj.UserEducation.UniversityName)
                {
                    if (user_obj.UserEducation.FacultyId.ToString().ToLower() == priend_obj.UserEducation.FacultyId.ToString().ToLower())
                    {
                        if (user_obj.UserEducation.Graduation.ToString() == priend_obj.UserEducation.Graduation.ToString())
                        {
                            common_education = 5;
                        }
                        else
                        {
                            common_education = 4;
                        }
                    }
                    else
                    {
                        if (user_obj.UserEducation.Graduation.ToString() == priend_obj.UserEducation.Graduation.ToString())
                        {
                            common_education = 3;
                        }
                        else
                        {
                            common_education = 2;
                        }
                    }
                }
                else
                {
                    if (user_obj.UserEducation.Graduation.ToString() == priend_obj.UserEducation.Graduation.ToString())
                    {
                        common_education = 1;
                    }
                }
            }

            result += common_education.ToString() + " ";

            int common_universities = 0;
            try
            {
                common_universities = user_obj.UserUniversities.Intersect(priend_obj.UserUniversities).Count();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("У одного из пользователей нет информации об университетах");
            }

            result += common_universities.ToString() + " ";

            int common_schools = 0;
            try
            {
                for (int i = 0; i < priend_obj.UserSchools.Count; i++)
                {
                    if (user_obj.UserSchools[0].Name.ToString().ToLower() == priend_obj.UserSchools[i].Name.ToString().ToLower())
                    {
                        common_schools = 1;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("У одного из пользователей нет информации о школах");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("У одного из пользователей нет информации о школах");
            }

            result += common_schools.ToString() + " ";

            if (user_obj.UserActivities != null && priend_obj.UserActivities != null)
            {
                string[] user_activities = user_obj.UserActivities.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').Split(' ');
                string[] priend_activities = priend_obj.UserActivities.Replace('.', ' ').Replace(',', ' ').Replace('!', ' ').Replace('?', ' ').Replace(':', ' ').Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Replace('"', ' ').Replace('#', ' ').Replace('/', ' ').Replace('>', ' ').Replace('<', ' ').Replace('@', ' ').Replace(';', ' ').ToLower().Split(' ');
                user_activities.ToList();
                priend_activities.ToList();
                try
                {
                    if (user_activities.Length > 0 && priend_activities.Length > 0 && int.TryParse(user_activities.Intersect(priend_activities).Count().ToString(), out int activities_count) && activities_count > 0)
                    {
                        result += activities_count.ToString() + " " + ((priend_activities.Length + user_activities.Length) / 2).ToString() + " ";
                    }
                    else
                    {
                        result += "0 0 ";
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("У одного из пользователей нет информации об университетах");
                    result += "0 0 ";
                }
            }
            else
            {
                result += "0 0 ";
            }

            int common_occupation = 0;

            if (user_obj.UserOccupation != null && user_obj.UserOccupation.Type != null && priend_obj.UserOccupation != null && priend_obj.UserOccupation.Type != null)
            {
                if (user_obj.UserOccupation.Type.ToString().ToLower() == priend_obj.UserOccupation.Type.ToString().ToLower())
                {
                    common_occupation = 1;
                }
            }
            result += common_occupation.ToString();
            return result;

        }

        /// <summary>
        /// Метод, позволяющий собрать первую часть данных о пользователях, в которую входят id, возраст, семейное положение и жизненная позиция.
        /// </summary>
        /// <param name="user1">Первый пользователь в паре.</param>
        /// <param name="user2"> Второй пользователь в паре.</param>
        /// <returns>Строка с целочисленными данными, разделёнными пробелом.</returns>
        public string CountFirstPart(User user1, User user2)
        {
            string result = "";
            result += user1.UserId + " ";
            result += user1.UserAge + " ";
            result += ConvertWordsToInt(user1.UserRelation.ToString()) + " ";
            if (user1.UserStandInLife != null)
            {
                result += ConvertWordsToInt(user1.UserStandInLife.Political.ToString()) + " ";
                result += ConvertWordsToInt(user1.UserStandInLife.Smoking.ToString()) + " ";
                result += ConvertWordsToInt(user1.UserStandInLife.Alcohol.ToString()) + " ";
                result += ConvertWordsToInt(user1.UserStandInLife.LifeMain.ToString()) + " ";
                result += ConvertWordsToInt(user1.UserStandInLife.PeopleMain.ToString()) + " ";
            }
            else
            {
                result += "0 0 0 0 0 ";
            }
            result += user2.UserId + " ";
            result += user2.UserAge + " ";
            result += ConvertWordsToInt(user2.UserRelation.ToString()) + " ";
            if (user2.UserStandInLife != null)
            {
                result += ConvertWordsToInt(user2.UserStandInLife.Political.ToString()) + " ";
                result += ConvertWordsToInt(user2.UserStandInLife.Smoking.ToString()) + " ";
                result += ConvertWordsToInt(user2.UserStandInLife.Alcohol.ToString()) + " ";
                result += ConvertWordsToInt(user2.UserStandInLife.LifeMain.ToString()) + " ";
                result += ConvertWordsToInt(user2.UserStandInLife.PeopleMain.ToString()) + " ";
            }
            else
            {
                result += "0 0 0 0 0 ";
            }
            return result;
        }

        /// <summary>
        /// Метод, позволяющий собрать все необходимые для работы нейросети данные о двух пользователях и их взаимодействии друг с другом.
        /// </summary>
        /// <param name="api">Экземпляр объекта для доступа к API.</param>
        /// <param name="user1">Первый пользователь из пары.</param>
        /// <param name="user2">Второй пользователь из пары.</param>
        /// <returns>Строка с целочисленными данными, разделёнными пробелами.</returns>
        public string GetInputData(VkApi api, User user1, User user2)
        {
            try
            {
                string line = "";
                Thread.Sleep(400);
                var groups1 = api.Users.GetSubscriptions(user1.UserId, 200, 0, GroupsFields.Activity).ToList();
                Thread.Sleep(400);
                var groups2 = api.Users.GetSubscriptions(user2.UserId, 200, 0, GroupsFields.Activity).ToList();
                Thread.Sleep(400);
                long couser1_audios = api.Audio.GetCount(user1.UserId);
                long couser2_audios = api.Audio.GetCount(user2.UserId);
                int coc_music = 0;
                int coc_artists = 0;
                List<long> groups1_ids = new List<long>() { };
                List<long> groups2_ids = new List<long>() { };
                List<string> groups1_activities = new List<string>() { };
                List<string> groups2_activities = new List<string>() { };
                for (int i = 0; i < groups1.Count; i++)
                {
                    groups1_ids.Add(groups1[i].Id);
                    if (groups1[i].Activity != "Открытая группа" && groups1[i].Activity != "Закрытая группа" && groups1[i].Activity != "Частная группа")
                    {
                        groups1_activities.Add(groups1[i].Activity);
                    }
                }

                for (int i = 0; i < groups2.Count; i++)
                {
                    groups2_ids.Add(groups2[i].Id);
                    if (groups2[i].Activity != "Открытая группа" && groups2[i].Activity != "Закрытая группа" && groups2[i].Activity != "Частная группа")
                    {
                        groups2_activities.Add(groups2[i].Activity);
                    }
                }

                int count_of_common_groups = 0;
                int common_activities_count = 0;

                if (groups1_ids != null && groups2_ids != null)
                {
                    count_of_common_groups = groups1_ids.Intersect(groups2_ids).Count();
                    common_activities_count = groups1_activities.Intersect(groups2_activities).Count();
                }
                line += groups1.Count.ToString() + " ";
                line += groups2.Count.ToString() + " ";
                line += count_of_common_groups.ToString() + " ";
                line += common_activities_count.ToString() + " ";
                line += couser1_audios.ToString() + " ";
                line += couser2_audios.ToString() + " ";
                line += coc_music.ToString() + " ";
                line += coc_artists.ToString() + " ";
                try
                {
                    line += CountAdditionalParameters(api, user1, user2);
                }
                catch (Exception)
                {
                    line += "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0";
                }
                return CountFirstPart(user1, user2) + line;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}