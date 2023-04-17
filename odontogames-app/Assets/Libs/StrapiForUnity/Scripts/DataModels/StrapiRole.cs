using System;
using UnityEngine;

namespace StrapiForUnity
{
    [System.Serializable]
    public class StrapiRole
    {
        public int id;
        public string name;
        public string description;
        public string type;

        [SerializeField]
        private string createdAt;
        private DateTime? _createdAt;
        public DateTime? CreatedAt()
        {
            if (_createdAt == null)
            {
                _createdAt = Convert.ToDateTime(createdAt);
            }
            return _createdAt;
        }

        [SerializeField]
        private string updatedAt;
        private DateTime? _updatedAt;
        public DateTime? UpdatedAt()
        {
            if (_updatedAt == null)
            {
                _updatedAt = Convert.ToDateTime(updatedAt);
            }
            return _updatedAt;
        }
    }
}
