﻿using System;

namespace SeiyuuDB.Entities {
  public interface ISeiyuuEntity<T> {
    int Id { get; set; }
    DateTime CreatedAt { get; set;  }
    DateTime UpdatedAt { get; set; }
    bool IsReadyEntity();
    bool IsReadyEntityWithoutId();
    void Replace(T entity);
  }
}
