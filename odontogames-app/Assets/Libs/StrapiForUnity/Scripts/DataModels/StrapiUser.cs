﻿﻿using System;
using UnityEngine;

 namespace StrapiForUnity
{
    [System.Serializable]
    public class StrapiUser {
        public int id;
        public string username;
        public string email;
        public string password;
        public string provider;
        public bool confirmed;
        public string firstname;
        public string lastname;
        public int firstgamescore;
        public int secondgamescore;
        public int thirdgamescore;
        public int fourthgamescore;
        public int fifthgamescore;
        public int sixthgamescore;
        public int seventhgamescore;
        public string userrole;
        public string group;

        [SerializeField]
        private string createdAt;
        private DateTime? _createdAt;
        public DateTime? CreatedAt ()
        {
            if (_createdAt == null) {
                _createdAt = Convert.ToDateTime (createdAt);
            }
            return _createdAt;
        }

        [SerializeField]
        private string updatedAt;
        private DateTime? _updatedAt;
        public DateTime? UpdatedAt ()
        {
            if (_updatedAt == null) {
                _updatedAt = Convert.ToDateTime (updatedAt);
            }
            return _updatedAt;
        }

        public StrapiRole role;
        public StrapiUserTeam team;
    }

    [Serializable]
    public class UserResponse
    {
        public StrapiUser[] users;
    }
}
