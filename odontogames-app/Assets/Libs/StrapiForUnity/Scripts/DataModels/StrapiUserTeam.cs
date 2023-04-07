using System;
using UnityEngine;

namespace StrapiForUnity
{
    [System.Serializable]
    public class StrapiTeamMember
    {
        public int id;
        public StrapiUser attributes;
    }

    [System.Serializable]
    public class StrapiTeamMembersData
    {
        public StrapiUserTeam[] data;
    }

    [System.Serializable]
    public class StrapiUserTeam
    {
        public int id;
        public string teamname;
        public int teamscore;
        public int numplayers;
        public string creator;

        [SerializeField]
        private string created_at;
        private DateTime? _createdAt;
        public DateTime? CreatedAt()
        {
            if (_createdAt == null)
            {
                _createdAt = Convert.ToDateTime(created_at);
            }
            return _createdAt;
        }

        [SerializeField]
        private string updated_at;
        private DateTime? _updatedAt;
        public DateTime? UpdatedAt()
        {
            if (_updatedAt == null)
            {
                _updatedAt = Convert.ToDateTime(updated_at);
            }
            return _updatedAt;
        }

        public StrapiTeamMembersData members;
    }

    [System.Serializable]
    public class StrapiTeamsData
    {
        public int id;
        public StrapiUserTeam attributes;
    }

    [Serializable]
    public class StrapiUserTeamListResponse
    {
        public StrapiTeamsData[] data;
    }
}
