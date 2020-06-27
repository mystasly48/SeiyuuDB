using SeiyuuDB.Entities;

namespace SeiyuuDB.Databases {
  public interface ISeiyuuDB {
    int Insert<T>(T entity) where T : class, ISeiyuuEntity<T>;
    int Update<T>(T entity) where T : class, ISeiyuuEntity<T>;
    int Delete<T>(T entity) where T : class, ISeiyuuEntity<T>;
    bool IsExists<T>(T entity) where T : class, ISeiyuuEntity<T>;
    T[] GetTableArray<T>() where T : class, ISeiyuuEntity<T>;
    T GetEntity<T>(int id) where T : class, ISeiyuuEntity<T>;
  }
}
