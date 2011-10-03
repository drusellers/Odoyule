// Copyright 2011 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace OdoyuleRules.Parsing
{
    using System.Linq;

    public abstract class RuleParser<TInput> :
        AbstractCharacterParser<TInput>
    {
        protected RuleParser()
        {
            Whitespace = Rep(Char(' ').Or(Char('\t').Or(Char('\n')).Or(Char('\r'))));
            NewLine = Rep(Char('\r').And(Char('\n')).Or(Char('\n')));
            Printable = Rep(Char(char.IsLetterOrDigit).Or(Char(char.IsWhiteSpace)).Or(Char('.')).Or(Char(',')));

            Id = from w in Whitespace
                 from c in Char(char.IsLetter)
                 from cs in Rep(Char(char.IsLetterOrDigit))
                 select new string(new[] {c}.Concat(cs).ToArray());

            QuotedString = from w in Whitespace
                           from enq in Char('"').Or(Char('\''))
                           from text in Printable
                           from deq in Char(enq)
                           select new string(text);

            Minus = from w in Whitespace
                    from op in Char('-')
                    select (Operator)new MinusOperator();

            Plus = from w in Whitespace
                   from op in Char('+')
                   select (Operator)new PlusOperator();

            Multiply = from w in Whitespace
                       from op in Char('*')
                       select (Operator)new MultiplyOperator();

            Divide = from w in Whitespace
                     from op in Char('/')
                     select (Operator)new DivideOperator();

            Equal = from w in Whitespace
                     from op in Char('=').And(Char('='))
                     select (Comparator)new EqualComparator();

            NotEqual = from w in Whitespace
                     from op in Char('!').And(Char('='))
                       select (Comparator)new NotEqualComparator();

            GreaterThanOrEqual = from w in Whitespace
                     from op in Char('>').And(Char('='))
                     select (Comparator)new GreaterThanOrEqualComparator();

            LessThanOrEqual = from w in Whitespace
                     from op in Char('<').And(Char('='))
                       select (Comparator)new LessThanOrEqualComparator();

            LessThan = from w in Whitespace
                     from op in Char('<')
                     select (Comparator)new LessThanComparator();

            GreaterThan = from w in Whitespace
                     from op in Char('<')
                     select (Comparator)new GreaterThanComparator();

            Operators = Multiply.Or(Divide).Or(Plus).Or(Minus);

            Comparators = NotEqual.Or(GreaterThanOrEqual).Or(LessThanOrEqual).Or(Equal).Or(GreaterThan).Or(LessThan);

            Condition = from w in Whitespace
                        from id in Id
                        from op in Comparators
                        from value in QuotedString
                        select new RuleCondition(id, op, value);

            Rule = from open in Id
                   where open == "rule"
                   from name in QuotedString.Or(Id)
                   from when in Id
                   where when == "when"
                   from conditions in Rep(Condition)
                   from then in Id
                   where then == "then"
                   from theEnd in Id
                   where theEnd == "end"
                   select new RuleDefinition(name, conditions);
        }

        public Parser<TInput, Comparator> Equal { get; private set; }
        public Parser<TInput, Comparator> NotEqual { get; private set; }
        public Parser<TInput, Comparator> GreaterThan { get; private set; }
        public Parser<TInput, Comparator> GreaterThanOrEqual { get; private set; }
        public Parser<TInput, Comparator> LessThan { get; private set; }
        public Parser<TInput, Comparator> LessThanOrEqual { get; private set; }
        public Parser<TInput, Comparator> Comparators { get; private set; }

        public Parser<TInput, Operator> Multiply { get; private set; }
        public Parser<TInput, Operator> Divide { get; private set; }
        public Parser<TInput, Operator> Plus { get; private set; }
        public Parser<TInput, Operator> Minus { get; private set; }
        public Parser<TInput, Operator> Operators { get; private set; }

        public Parser<TInput, char[]> Whitespace { get; private set; }

        public Parser<TInput, char[]> NewLine { get; private set; }

        public Parser<TInput, string> Id { get; private set; }

        public Parser<TInput, string> QuotedString { get; private set; }

        public Parser<TInput, char[]> Printable { get; private set; }

        public Parser<TInput, RuleDefinition> Rule { get; private set; }

        public Parser<TInput, RuleCondition> Condition { get; private set; }
    }
}