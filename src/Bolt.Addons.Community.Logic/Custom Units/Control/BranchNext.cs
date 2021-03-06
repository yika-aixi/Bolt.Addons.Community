﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ludiq;

namespace Bolt
{
    /// <summary>
    /// Branches flow by checking if a condition is true or false.
    /// </summary>
    [UnitCategory("Control")]
    [UnitOrder(0)]
    public sealed class BranchNext : Unit, IBranchUnit
    {
        public BranchNext() : base() { }

        /// <summary>
        /// The entry point for the branch.
        /// </summary>
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput enter { get; private set; }

        /// <summary>
        /// The condition to check.
        /// </summary>
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput condition { get; private set; }

        /// <summary>
        /// The action to execute if the condition is true.
        /// </summary>
        [DoNotSerialize]
        [PortLabel("True")]
        public ControlOutput ifTrue { get; private set; }

        /// <summary>
        /// The action to execute if the condition is false.
        /// </summary>
        [DoNotSerialize]
        [PortLabel("False")]
        public ControlOutput ifFalse { get; private set; }


        /// <summary>
        /// The action to execute if the condition is false.
        /// </summary>
        [DoNotSerialize]
        [PortLabel("Next")]
        public ControlOutput onNext { get; private set; }

        protected override void Definition()
        {
            enter = ControlInput("enter", Enter);
            condition = ValueInput<bool>("condition");
            ifTrue = ControlOutput("ifTrue");
            ifFalse = ControlOutput("ifFalse");
            onNext = ControlOutput("onNext");

            Relation(enter, ifTrue);
            Relation(enter, ifFalse);
            Relation(enter, onNext);
            Relation(condition, enter);
        }

        public void Enter(Flow flow)
        {
            if (condition.GetValue<bool>())
            {
                flow.Invoke(ifTrue);
            }
            else
            {
                flow.Invoke(ifFalse);
            }
            flow.Invoke(onNext);
        }
    }
}