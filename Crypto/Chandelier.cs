using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
    public class Chandelier
    {
        public Double openTime { get; set; }
        public String open { get; set; }
        public String high { get; set; }
        public String low { get; set; }
        public String close { get; set; }
        public String volume { get; set; }
        public Double closeTime { get; set; }
        public String quoteAssetVolume { get; set; }
        public Double numberOfTrades { get; set; }
        public String takerBuyBaseAssetVolume { get; set; }
        public String takerBuyQuoteAssetVolume { get; set; }
        public String canBeIgnored { get; set; }

        public String evolution { get; set; }
        public String evolutionExtrema { get; set; }
    }
}
