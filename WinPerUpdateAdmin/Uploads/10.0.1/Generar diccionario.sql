select '<?xml version="1.0" encoding="utf-8"?><Language Name="Espa�ol">'

union

select '<LocaleResource Name="' + Name + '"><Value>' + Value + '</Value></LocaleResource>'
from   espa�ol

union

select '</Language>'

update espa�ol
set    Value = REPLACE(Value, '<', '&lt;')

update espa�ol
set    Value = REPLACE(Value, '>', '&gt;')

