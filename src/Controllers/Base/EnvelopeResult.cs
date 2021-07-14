using System;
using System.Collections.Generic;
using System.Linq;
using DasTeamRevolution.Models.Dto;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;

namespace DasTeamRevolution.Controllers.Base
{
    public class EnvelopeResult
    {
        private readonly ControllerBase _controllerBase;

        public EnvelopeResult(ControllerBase controllerBase)
        {
            _controllerBase = controllerBase;
        }

        public OkObjectResult Ok<T>(IEnumerable<T> items)
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }

        public OkObjectResult Ok<T>(IList<T> items)
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }

        public OkObjectResult Ok<T>(List<T> items)
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }
        
        public OkObjectResult Ok<T>(T item)
        {
            return _okArray(new []{item});
        }
        
        private OkObjectResult _okArray<T>(IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];

            IODataFeature oDataFeature = _controllerBase.HttpContext.ODataFeature();
            
            return _controllerBase.Ok(new ResponseBodyDto
            {
                Data = new ResponseBodyDataDto
                {
                    Type =  itemType?.Name,
                    Items = items.Cast<object>(),
                    Count = oDataFeature.TotalCount
                }
            });
        }

        public CreatedResult Created<T>(string url, IEnumerable<T> items)
        {
            if (items is null)
                return _createdArray(url, Array.Empty<T>());

            return _createdArray(url, items);
        }
        
        public CreatedResult Created<T>(string url, T item) where T:class
        {
            return _createdArray(url, new[] {item});
        }
        
        private CreatedResult _createdArray<T>(string url, IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();

            return _controllerBase.Created(url, new ResponseBodyDto
            {
                Data = new ResponseBodyDataDto
                {
                    Type = itemType?.Name,
                    Items = enumerable.Cast<object>(),
                    Count = enumerable.LongCount()
                }
            });
        }

        public OkObjectResult Updated<T>(T item) where T:class
        {
            return _updatedArray(new[] {item});
        }

        public OkObjectResult Updated<T>(IEnumerable<T> items)
        {
            if (items is null)
                return _updatedArray(Array.Empty<T>());

            return _updatedArray(items);
        }

        private OkObjectResult _updatedArray<T>(IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();

            return _controllerBase.Ok(new ResponseBodyDto
            {
                Data = new ResponseBodyDataDto
                {
                    Type = itemType?.Name,
                    Items = enumerable.Cast<object>(),
                    Count = enumerable.LongCount()
                }
            });
        }
    }
}