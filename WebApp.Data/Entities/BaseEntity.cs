using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Data.Entities
{
    public class BaseEntity
    {
        #region Members

        private Boolean _isActive = true;

        private Guid _createdByUserId;

        private DateTime _createdDate = DateTime.Now;

        private Guid? _updatedByUserId;

        private DateTime? _updatedDate;

        private Guid? _restoredByUserId;

        private DateTime? _restoredDate;

        private Guid? _deletedByUserId;

        private DateTime? _deletedDate;

        #endregion Members

        #region Properties

        public Boolean IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Guid CreatedByUserId
        {
            get { return _createdByUserId; }
            set { _createdByUserId = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public Guid? UpdatedByUserId
        {
            get { return _updatedByUserId; }
            set { _updatedByUserId = value; }
        }

        public DateTime? UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        public Guid? RestoredByUserId
        {
            get { return _restoredByUserId; }
            set { _restoredByUserId = value; }
        }

        public DateTime? RestoredDate
        {
            get { return _restoredDate; }
            set { _restoredDate = value; }
        }

        public Guid? DeletedByUserId
        {
            get { return _deletedByUserId; }
            set { _deletedByUserId = value; }
        }

        public DateTime? DeletedDate
        {
            get { return _deletedDate; }
            set { _deletedDate = value; }
        }

        #endregion Properties
    }
}
