using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.tcl.common.storage.Entities
{
    /// <summary>
    /// Abstract class for sql lite database items used in the app.
    /// </summary>
    public abstract class SqLiteDatabaseItem
    {

        #region =============== properties =================================

        /// <summary>
        /// Primary key of the database item: Simple id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        #endregion ============ properties =================================

        #region =============== public & internal methods ==================

        /// <summary>
        /// Makes easy for comparison based on the <see cref="Id"/>.
        /// </summary>Q
        /// <param name="obj">Object to compare.</param>
        /// <returns><c>True</c>, if both objects have the same <see cref="Id"/>, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            SqLiteDatabaseItem toCompare = obj as SqLiteDatabaseItem;
            if (toCompare == null)
            {
                return false;
            }

            return Id == toCompare.Id;
        }

        /// <summary>
        /// Returns hash code based on the <see cref="Id"/>.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion ============ public & internal methods ==================

    }
}
