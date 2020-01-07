# DNS C#
## Program launch
Args:
- args[0] - DNS address
- args[1] - domain
- args[3] - resource record type

Example:

    > dns-csharp 8.8.8.8 example.com TXT
    NAME                     TYPE                     RDATA
    example.com              TXT                      v=spf1 -all

Реализована обработка RDATA для записей типов:
- A
- TXT
- MX

Для остальных типов нужно добавить свою обработку в методе Program.ParseData.
