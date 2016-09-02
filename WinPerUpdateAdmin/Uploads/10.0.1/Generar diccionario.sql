select '<?xml version="1.0" encoding="utf-8"?><Language Name="Español">'

union

select '<LocaleResource Name="' + Name + '"><Value>' + Value + '</Value></LocaleResource>'
from   español

union

select '</Language>'

update español
set    Value = REPLACE(Value, '<', '&lt;')

update español
set    Value = REPLACE(Value, '>', '&gt;')

