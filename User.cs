using System;
using System.Collections.ObjectModel;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace SmartFriendShipper
{
    /// <summary>
    /// Класс, описывающий пользователя социальной сети "ВКонтакте".
    /// </summary>
    public class User
    {
        // Защищённые поля класса
        protected long user_id;
        protected string user_name;
        protected string user_surname;
        protected int user_age;
        protected string user_status;
        protected string user_bdate;
        protected string user_sex;
        protected string user_about;
        protected string user_activities;
        protected string user_books;
        protected string user_domain;
        protected string user_homephone;
        protected string user_hometown;
        protected string user_interests;
        protected string user_maidenname;
        protected string user_mobilephone;
        protected string user_movies;
        protected string user_music;
        protected string user_nickname;
        protected string user_photoid;
        protected string user_site;
        protected string user_tv;
        protected string user_quotes;
        protected string user_games;
        protected bool user_canSeeAllPosts;
        protected bool user_canSeeAudio;
        protected bool? user_isClosed;
        protected bool user_isDeactivated;
        protected bool? user_hasPhoto;
        protected bool? user_online;
        protected long? user_onlineApp;
        protected bool? user_onlineMobile;
        protected bool user_wallComments;
        protected bool? user_verified;
        protected long? user_followersCount;
        protected ReadOnlyCollection<Career> user_career;
        protected City user_city;
        protected Connections user_connections;
        protected Contacts user_contacts;
        protected Counters user_counters;
        protected Country user_country;
        protected Education user_education;
        protected Exports user_exports;
        protected LastSeen user_lastSeen;
        protected Military user_military;
        protected Occupation user_occupation;
        protected Uri user_photo100;
        protected Uri user_photo50;
        protected Previews user_photoPreviews;
        protected User user_relationPartner;
        protected VkNet.Enums.RelationType user_relation;
        protected ReadOnlyCollection<Relative> user_relatives;
        protected ReadOnlyCollection<School> user_schools;
        protected StandInLife user_standInLife;
        protected Audio user_audioStatus;
        protected int? user_timezone;
        protected ReadOnlyCollection<University> user_universities;
        protected WallGetObject user_wall;
        protected WallGetCommentsResult user_wallCommentsList;

        // Публичные свойства класса, с помощью которых организован доступ к защищённым полям.
        public VkNet.Enums.RelationType UserRelation
        {
            get
            {
                return user_relation;
            }
            set
            {
                user_relation = value;
            }
        }

        public int? UserTimezone
        {
            get
            {
                return user_timezone;
            }
            set
            {
                user_timezone = value;
            }
        }

        public Audio UserAudioStatus
        {
            get
            {
                return user_audioStatus;
            }
            set
            {
                user_audioStatus = value;
            }
        }

        public StandInLife UserStandInLife
        {
            get
            {
                return user_standInLife;
            }
            set
            {
                user_standInLife = value;
            }
        }

        public ReadOnlyCollection<Relative> UserRelatives
        {
            get
            {
                return user_relatives;
            }
            set
            {
                user_relatives = value;
            }
        }

        public ReadOnlyCollection<School> UserSchools
        {
            get
            {
                return user_schools;
            }
            set
            {
                user_schools = value;
            }
        }

        public ReadOnlyCollection<University> UserUniversities
        {
            get
            {
                return user_universities;
            }
            set
            {
                user_universities = value;
            }
        }

        public long UserId
        {
            get
            {
                return user_id;
            }
        }
        public string UserName
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
            }
        }
        public string UserSurname
        {
            get
            {
                return user_surname;
            }
            set
            {
                user_surname = value;
            }
        }

        public string UserBdate
        {
            get
            {
                return user_bdate;
            }
            set
            {
                if (value != "-1")
                {
                    user_bdate = value;
                }
                else
                {
                    user_bdate = "-1";
                }
            }
        }
        public int UserAge
        {
            get
            {
                return user_age;
            }
            set
            {
                DefineAge(value);
            }
        }

        public string UserSex
        {
            get
            {
                return user_sex;
            }
            set
            {
                user_sex = value;
            }
        }

        public string UserAbout
        {
            get
            {
                return user_about;
            }
            set
            {
                user_about = value;
            }
        }

        public string UserAcivities
        {
            get
            {
                return user_about;
            }
            set
            {
                user_about = value;
            }
        }
        public string UserActivities
        {
            get
            {
                return user_activities;
            }
            set
            {
                user_activities = value;
            }
        }
        public string UserBooks
        {
            get
            {
                return user_books;
            }
            set
            {
                user_books = value;
            }
        }
        public string UserDomain
        {
            get
            {
                return user_domain;
            }
            set
            {
                user_domain = value;
            }
        }

        public string UserHomePhone
        {
            get
            {
                return user_homephone;
            }
            set
            {
                user_homephone = value;
            }
        }

        public string UserHomeTown
        {
            get
            {
                return user_hometown;
            }
            set
            {
                user_hometown = value;
            }
        }

        public string UserMaidenName
        {
            get
            {
                return user_maidenname;
            }
            set
            {
                user_maidenname = value;
            }
        }

        public string UserMobilePhone
        {
            get
            {
                return user_mobilephone;
            }
            set
            {
                user_mobilephone = value;
            }
        }

        public string UserMovies
        {
            get
            {
                return user_movies;
            }
            set
            {
                user_movies = value;
            }
        }

        public string UserMusic
        {
            get
            {
                return user_music;
            }
            set
            {
                user_music = value;
            }
        }

        public string UserNickname
        {
            get
            {
                return user_nickname;
            }
            set
            {
                user_nickname = value;
            }
        }

        public string UserPhotoId
        {
            get
            {
                return user_photoid;
            }
            set
            {
                user_photoid = value;
            }
        }

        public string UserTv
        {
            get
            {
                return user_tv;
            }
            set
            {
                user_tv = value;
            }
        }

        public string UserSite
        {
            get
            {
                return user_site;
            }
            set
            {
                user_site = value;
            }
        }

        public string UserStatus
        {
            get
            {
                return user_status;
            }
            set
            {
                user_status = value;
            }
        }

        public string UserQuotes
        {
            get
            {
                return user_quotes;
            }
            set
            {
                user_quotes = value;
            }
        }

        public string UserInterests
        {
            get
            {
                return user_interests;
            }
            set
            {
                user_interests = value;
            }
        }

        public string UserGames
        {
            get
            {
                return user_games;
            }
            set
            {
                user_games = value;
            }
        }
        public bool UserCanSeeAudio
        {
            get
            {
                return user_canSeeAudio;
            }
            set
            {
                user_canSeeAudio = value;
            }
        }
        public bool UserCanSeeAllPosts
        {
            get
            {
                return user_canSeeAllPosts;
            }
            set
            {
                user_canSeeAllPosts = value;
            }
        }
        public bool UserIsDeactivated
        {
            get
            {
                return user_isDeactivated;
            }
            set
            {
                user_isDeactivated = value;
            }
        }
        public bool? UserVerified
        {
            get
            {
                return user_verified;
            }
            set
            {
                user_verified = value;
            }
        }

        public bool? UserHasPhoto
        {
            get
            {
                return user_hasPhoto;
            }
            set
            {
                user_hasPhoto = value;
            }
        }
        public bool? UserIsClosed
        {
            get
            {
                return user_isClosed;
            }
            set
            {
                user_isClosed = value;
            }
        }
        public bool UserWallComments
        {
            get
            {
                return user_wallComments;
            }
            set
            {
                user_wallComments = value;
            }
        }
        public bool? UserOnline
        {
            get
            {
                return user_online;
            }
            set
            {
                user_online = value;
            }
        }
        public long? UserOnlineApp
        {
            get
            {
                return user_onlineApp;
            }
            set
            {
                user_onlineApp = value;
            }
        }
        public bool? UserOnlineMobile
        {
            get
            {
                return user_onlineMobile;
            }
            set
            {
                user_onlineMobile = value;
            }
        }

        public ReadOnlyCollection<Career> UserCareer
        {
            get
            {
                return user_career;
            }
            set
            {
                user_career = value;
            }
        }

        public City UserCity
        {
            get
            {
                return user_city;
            }
            set
            {
                user_city = value;
            }
        }

        public WallGetObject UserWall
        {
            get
            {
                return user_wall;
            }
            set
            {
                user_wall = value;
            }
        }

        public WallGetCommentsResult UserWallCommentsList
        {
            get
            {
                return user_wallCommentsList;
            }
            set
            {
                user_wallCommentsList = value;
            }
        }

        public Connections UserConnections
        {
            get
            {
                return user_connections;
            }
            set
            {
                user_connections = value;
            }
        }

        public Contacts UserContacts
        {
            get
            {
                return user_contacts;
            }
            set
            {
                user_contacts = value;
            }
        }

        public Counters UserCounters
        {
            get
            {
                return user_counters;
            }
            set
            {
                user_counters = value;
            }
        }

        public Country UserCountry
        {
            get
            {
                return user_country;
            }
            set
            {
                user_country = value;
            }
        }

        public Education UserEducation
        {
            get
            {
                return user_education;
            }
            set
            {
                user_education = value;
            }
        }

        public Exports UserExports
        {
            get
            {
                return user_exports;
            }
            set
            {
                user_exports = value;
            }
        }

        public LastSeen UserLastSeen
        {
            get
            {
                return user_lastSeen;
            }
            set
            {
                user_lastSeen = value;
            }
        }

        public Military UserMilitary
        {
            get
            {
                return user_military;
            }
            set
            {
                user_military = value;
            }
        }

        public Occupation UserOccupation
        {
            get
            {
                return user_occupation;
            }
            set
            {
                user_occupation = value;
            }
        }

        public Uri UserPhoto100
        {
            get
            {
                return user_photo100;
            }
            set
            {
                user_photo100 = value;
            }
        }

        public Uri UserPhoto50
        {
            get
            {
                return user_photo50;
            }
            set
            {
                user_photo50 = value;
            }
        }

        public Previews UserPhotoPreviews
        {
            get
            {
                return user_photoPreviews;
            }
            set
            {
                user_photoPreviews = value;
            }
        }

        public User UserRelationPartner
        {
            get
            {
                return user_relationPartner;
            }
            set
            {
                user_relationPartner = value;
            }
        }

        public long? UserFollowersCount
        {
            get
            {
                return user_followersCount;
            }
            set
            {
                user_followersCount = value;
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="_user_id">Идентификатор пользователя в социальной сети.</param>
        public User(long _user_id)
        {
            user_id = _user_id;
        }

        /// <summary>
        /// Метод, позволяющий определить возраст пользователя по его дате рождения.
        /// </summary>
        /// <param name="value"></param>
        private void DefineAge(int value)
        {
            if (user_bdate != null)
            {
                string[] bdate = user_bdate.Split('.');
                if (bdate.Length == 3)
                {
                    user_age = DateTime.Now.Year - int.Parse(bdate[2]);
                    if (DateTime.Now.Month < int.Parse(bdate[1]))
                    {
                        user_age -= 1;
                    }
                    if (DateTime.Now.Month == int.Parse(bdate[1]) && DateTime.Now.Day < int.Parse(bdate[0]))
                    {
                        user_age -= 1;
                    }
                }
                else
                {
                    user_age = value;
                }
            }
            else
            {
                user_age = value;
            }
        }
    }
}