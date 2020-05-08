using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WinHttp;

namespace solved_ac_calculator
{

    public class ProblemClient
    {
        public WinHttp.WinHttpRequest req = null;
        public String[] solves = null;
        public Tier tier = new Tier("Unknown", 0L, 1L, -1);
        public Int64 exp = 0L;
        public bool request_ok = false;

        public int[] diff = {
            256, //Unrated
            480, 672, 954, 1374, 1992, //Bronze
            2909, 4276, 6329, 9430, 14145, //Silver
            21288, 32145, 48699, 74023, 112885, //Gold
            172714, 265117, 408280, 630792, 977727, //Platinum
            1520366, 2371771, 3711822, 5827560, 9178407, //Diamond
            14501883, 22985485, 36546921, 58292339, 93267742 //Ruby
        };
        public struct Tier
        {
            public String name;
            public Int64 low, high;
            public int lv;

            public Tier(String name, Int64 low, Int64 high, int lv)
            {
                this.name = name;
                this.low = low;
                this.high = high;
                this.lv = lv;
            }
        }

        public Tier[] _tier = new Tier[] {
            new Tier("Bronze V", 0L, 9590L, 1),
            new Tier("Bronze IV", 9590L, 23030L, 2),
            new Tier("Bronze III", 23030L, 42110L, 3),
            new Tier("Bronze II", 42110L, 69590L, 4),
            new Tier("Bronze I", 69590L, 109430L, 5),
            new Tier("Silver V", 109430L, 182155L, 6),
            new Tier("Silver IV", 182155L, 289055L, 7),
            new Tier("Silver III", 289055L, 447280L, 8),
            new Tier("Silver II", 447280L, 683030L, 9),
            new Tier("Silver I", 683030L, 1036655L, 10),
            new Tier("Gold V", 1036655L, 1675295L, 11),
            new Tier("Gold IV", 1675295L, 2639645L, 12),
            new Tier("Gold III", 2639645L, 4100615L, 13),
            new Tier("Gold II", 4100615L, 6321305L, 14),
            new Tier("Gold I", 6321305L, 10836705L, 15),
            new Tier("Platinum V", 10836705L, 16018125L, 16),
            new Tier("Platinum IV", 16018125L, 23971635L, 17),
            new Tier("Platinum III", 23971635L, 36220035L, 18),
            new Tier("Platinum II", 36220035L, 55143795L, 19),
            new Tier("Platinum I", 55143795L, 84475605L, 20),
            new Tier("Diamond V", 84475605L, 130086585L, 21),
            new Tier("Diamond IV", 130086585L, 201244515L, 22),
            new Tier("Diamond III", 201244515L, 312599175L, 23),
            new Tier("Diamond II", 312599175L, 487425975L, 24),
            new Tier("Diamond I", 487425975L, 854562255L, 25),
            new Tier("Ruby V", 854562255L, 1434637575L, 26),
            new Tier("Ruby IV", 1434637575L, 2354056975L, 27),
            new Tier("Ruby III", 2354056975L, 3815933815L, 28),
            new Tier("Ruby II", 3815933815L, 6147627375L, 29),
            new Tier("Ruby I", 6147627375L, 999999999999L, 30)
        };

        public ProblemClient(TextBox text_input)
        {
            req = new WinHttpRequest();
            solves = text_input.Text.Split(' ');
            tier = new Tier("Unknown", 0L, 1L, -1);
            exp = 0L;
            request_ok = false;
        }

        public int request(ProgressBar request_bar)
        {
            int i = 1, cnt = 0;
            foreach (String solve in solves)
            {
                request_bar.Value = i * 100 / solves.Length;
                i++;

                if (String.IsNullOrWhiteSpace(solve)) continue;
                String pID = "http://api.solved.ac/problem_level.php?id=" + solve;
                req.Open("GET", pID, true);
                req.SetRequestHeader("User-Agent", "Mozilla/5.0");
                req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                req.Send();
                req.WaitForResponse();

                String res = req.ResponseText;
                if (res.Length < 10) continue;

                char lt = res[9], rt = res[10];
                int level;
                if (lt >= '0' && lt <= '9')
                {
                    if (rt >= '0' && rt <= '9')
                        level = (lt - '0') * 10 + (rt - '0');
                    else level = lt - '0';
                }
                else level = 0;
                exp += diff[level];
                if (diff[level] > 0) cnt++;
            }
            request_ok = true;
            return cnt;
        }

        public void calculate()
        {
            foreach (Tier i in _tier)
            {
                if (exp >= i.low && exp < i.high)
                {
                    tier = i;
                    break;
                }
            }
        }
    }
}
