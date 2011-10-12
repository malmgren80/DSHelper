using System.Text;

namespace DSHelper.Data.Sql
{
    public class ConditionsBuilder
    {
        private readonly StringBuilder _conditions;
        
        public ConditionsBuilder()
        {
            _conditions = new StringBuilder();
        }

        public ConditionsBuilder(string conditions)
        {
            _conditions = new StringBuilder(conditions);
        }

        public void Add(string condition)
        {
            string whereOrAnd = _conditions.Length == 0 ? "WHERE" : "AND";
            string conditionToAdd = string.Format(" {0} {1} ", whereOrAnd, condition);
            _conditions.Append(conditionToAdd);
        }

        public string Build()
        {
            return _conditions.ToString();
        }

        public override string ToString()
        {
            return _conditions.ToString();
        }
    }
}
