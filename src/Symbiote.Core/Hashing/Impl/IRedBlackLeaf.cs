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
    public interface IRedBlackLeaf<TKey, TValue>
    {
        LeafColor Color { get; set; }
        TKey Key { get; set; }
        IRedBlackLeaf<TKey, TValue> Parent { get; set; }
        TValue Value { get; set; }
        IRedBlackLeaf<TKey, TValue> Right { get; set; }
        IRedBlackLeaf<TKey, TValue> Left { get; set; }
        bool IsRoot { get; }
        int Count { get; }
        bool GreaterThan(IRedBlackLeaf<TKey, TValue> leaf);
        bool LessThan(IRedBlackLeaf<TKey, TValue> leaf);

        IRedBlackLeaf<TKey, TValue> this[bool right] { get; set; }
    }
}