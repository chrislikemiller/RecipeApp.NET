using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Domain.Exceptions;

public class EntityNotFoundException<T> : EntityException<T>
{
    private const string _message = "is not found";

    public EntityNotFoundException(int entityId, string entityName = "") : base(entityName, entityId, _message)
    {
    }

}

