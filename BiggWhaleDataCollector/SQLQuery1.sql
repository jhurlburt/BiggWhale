select distinct f.Name , f.[Ticker Symbol], f.[Total Net Assets], fa.[Top Funds], fa.[Top Funds Date], fd.NAV, fd.MarketPrice, fd.[Crawl Date]
from Funds f,FundAssets fa, FundDetails fd
where f.id = fa.fund_id
and f.id = fd.fund_id
and fa.fund_id = fd.fund_id
order by f.[Total Net Assets] desc,f.Name, fd.[Crawl Date]
