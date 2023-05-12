﻿using System;
using System.Collections;
using System.Collections.Generic;
using Expressions.Task3.E3SQueryProvider.Models.Entities;
using Expressions.Task3.E3SQueryProvider.Models.Request;

namespace Expressions.Task3.E3SQueryProvider.Services
{
    public interface IE3SSearchService
    {
        IEnumerable<T> SearchFts<T>(FtsQueryRequest query, int start = 0, int limit = 0) where T : BaseE3SEntity;

        IEnumerable SearchFts(Type type, FtsQueryRequest query, int start = 0, int limit = 0);
    }
}
