using System;
namespace CairoEngine.Scripting.Conditions
{
    public class Condition_CompareNumberVariable : Condition
    {
        /// <summary>
        /// Checks the First Field (Number Variable) against the Third Field (Number Value) based on the Second Field (Operation)
        /// </summary>
        public override bool Check()
        {
            switch (fields[1].Get<string>())
            {
                case ">":
                    value = fields[2].Get<float>() > (float)script.variables[fields[0].Get<string>()];
                    break;
                case ">=":
                    value = fields[2].Get<float>() >= (float)script.variables[fields[0].Get<string>()];
                    break;
                case "=":
                    value = fields[2].Get<float>() == (float)script.variables[fields[0].Get<string>()];
                    break;
                case "<":
                    value = fields[2].Get<float>() < (float)script.variables[fields[0].Get<string>()];
                    break;
                case "<=":
                    value = fields[2].Get<float>() <= (float)script.variables[fields[0].Get<string>()];
                    break;
            }

            return base.Check();
        }
    }
}
