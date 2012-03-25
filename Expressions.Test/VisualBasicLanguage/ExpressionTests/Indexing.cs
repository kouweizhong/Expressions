﻿using System;
using System.Collections.Generic;
using System.Text;
using Expressions.Expressions;
using NUnit.Framework;

namespace Expressions.Test.VisualBasicLanguage.ExpressionTests
{
    [TestFixture]
    internal class Indexing : TestBase
    {
        [Test]
        public void SimpleCollection()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "SimpleCollection(0)",
                new MethodCall(
                    new MethodCall(
                        new VariableAccess(typeof(Owner), 0),
                        typeof(Owner).GetMethod("get_SimpleCollection"),
                        null
                    ),
                    typeof(OwnerSimpleCollection).GetMethod("get_Item"),
                    new IExpression[]
                    {
                        new Constant(0)
                    }
                )
            );
        }

        [Test]
        public void CollectionWithOverload()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "DualCollection(0)",
                new MethodCall(
                    new MethodCall(
                        new VariableAccess(typeof(Owner), 0),
                        typeof(Owner).GetMethod("get_DualCollection"),
                        null
                    ),
                    typeof(OwnerDualCollection).GetMethod("get_Item", new[] { typeof(int) }),
                    new IExpression[]
                    {
                        new Constant(0)
                    }
                )
            );

            Resolve(
                new ExpressionContext(null, new Owner()),
                "DualCollection(\"key\")",
                new MethodCall(
                    new MethodCall(
                        new VariableAccess(typeof(Owner), 0),
                        typeof(Owner).GetMethod("get_DualCollection"),
                        null
                    ),
                    typeof(OwnerDualCollection).GetMethod("get_Item", new[] { typeof(string) }),
                    new IExpression[]
                    {
                        new Constant("key")
                    }
                )
            );
        }

        [Test]
        [ExpectedException]
        public void UnresolvedIndexWithOverload()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "DualCollection(1.7)"
            );
        }

        [Test]
        [ExpectedException]
        public void CannotIndexArrayWithDouble()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "SimpleArray(1.7)"
            );
        }

        [Test]
        [ExpectedException]
        public void NoIndexer()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "Value(0)"
            );
        }

        [Test]
        public void FieldIndex()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "SimpleArray(0)",
                new Index(
                    new MethodCall(
                        new VariableAccess(typeof(Owner), 0),
                        typeof(Owner).GetMethod("get_SimpleArray"),
                        null
                    ),
                    new Constant(0),
                    typeof(int)
                )
            );
        }

        [Test]
        public void RankedFieldIndex()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "RankedArray(0,0)",
                new MethodCall(
                    new MethodCall(
                        new VariableAccess(typeof(Owner), 0),
                        typeof(Owner).GetMethod("get_RankedArray"),
                        null
                    ),
                    typeof(int[,]).GetMethod("Get"),
                    new IExpression[]
                    {
                        new Constant(0),
                        new Constant(0)
                    }
                )
            );
        }

        [Test]
        [ExpectedException]
        public void IllegalRank()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "RankedArray(0,0,0)"
            );
        }

        [Test]
        public void IndexOnMember()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "Item.SimpleCollection(0)",
                new MethodCall(
                    new MethodCall(
                        new MethodCall(
                            new VariableAccess(typeof(Owner), 0),
                            typeof(Owner).GetMethod("get_Item"),
                            null
                        ),
                        typeof(Owner).GetMethod("get_SimpleCollection"),
                        null
                    ),
                    typeof(OwnerSimpleCollection).GetMethod("get_Item"),
                    new IExpression[]
                    {
                        new Constant(0)
                    }
                )
            );
        }

        [Test]
        public void IndexOnIndexed()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "SimpleCollection(0).SimpleCollection(0)",
                new MethodCall(
                    new MethodCall(
                        new MethodCall(
                            new MethodCall(
                                new VariableAccess(typeof(Owner), 0),
                                typeof(Owner).GetMethod("get_SimpleCollection"),
                                null
                            ),
                            typeof(OwnerSimpleCollection).GetMethod("get_Item"),
                            new IExpression[]
                            {
                                new Constant(0)
                            }
                        ),
                        typeof(Owner).GetMethod("get_SimpleCollection"),
                        null
                    ),
                    typeof(OwnerSimpleCollection).GetMethod("get_Item"),
                    new IExpression[]
                    {
                        new Constant(0)
                    }
                )
            );
        }

        [Test]
        public void MethodOnIndexed()
        {
            Resolve(
                new ExpressionContext(null, new Owner()),
                "SimpleCollection(0).Method()",
                new MethodCall(
                    new MethodCall(
                        new MethodCall(
                            new VariableAccess(typeof(Owner), 0),
                            typeof(Owner).GetMethod("get_SimpleCollection"),
                            null
                        ),
                        typeof(OwnerSimpleCollection).GetMethod("get_Item"),
                        new IExpression[]
                        {
                            new Constant(0)
                        }
                    ),
                    typeof(Owner).GetMethod("Method"),
                    null
                )
            );
        }

        public class Owner
        {
            public int Value { get; set; }

            public OwnerSimpleCollection SimpleCollection { get; set; }

            public OwnerDualCollection DualCollection { get; set; }

            public int[] SimpleArray { get; set; }

            public int[,] RankedArray { get; set; }

            public Owner Item { get; set; }

            public int Method()
            {
                return 0;
            }
        }

        public class OwnerSimpleCollection
        {
            public Owner this[int key]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }

        public class OwnerDualCollection
        {
            public int this[int key]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public string this[string key]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }
    }
}
