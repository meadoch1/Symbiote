﻿/* 
Copyright 2008-2010 Alex Robson

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

namespace Symbiote.Core.Hashing.Impl
{
    public class HashedAvlTree<TKey, TValue> 
        : AvlTreeBase<TKey, TValue>
    {
        protected IHashingProvider HashProvider { get; set; }

        protected override IAvlLeaf<TKey, TValue> CreateLeaf(TKey key, TValue value)
        {
            return new HashedAvlLeaf<TKey, TValue>()
                       {
                           Key = key,
                           Value = value,
                           HashKey = HashProvider.Hash(key),
                       };
        }

        public HashedAvlTree()
        {
            HashProvider = new MD5HashProvider();
        }
    }
}