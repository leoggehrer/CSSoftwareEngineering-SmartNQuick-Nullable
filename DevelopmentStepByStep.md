# Software-Entwicklung mit dem SmartNQuick   
  
## Erforderliche NuGet-Packages  
### Module Logic:  
* Microsoft.EntityFrameworkCore  
* Microsoft.EntityFrameworkCore.SqlServer  
* System.Linq.Dynamic.Core  
  
### Module ConApp  
* Microsoft.EntityFrameworkCore.Tools  
* Microsoft.EntityFrameworkCore.Design  
  
## ConnectionString  
In der Klasse 'ProjectDbContext' muss der ConnctionString gesetzt werden:  
ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=SmartNQuickDb;Integrated Security=True";  
  
## Definieren der Schnittstellen  
1. Schnittstelle definieren (interface)  
1.1 Persistence -> muss von ***IVersionable*** und ***ICopyable*** abgeleitet sein  
  
2. Entity implementieren im entsprechenden Ordner  
2.1 Ableiten von VersionEntity  
2.2 Eigenschaften erstellen  
2.3 CopyProperties(...) implementieren  
  
3. Entity beim ProjectDbContext registrieren  
3.1 Key setzen  
3.2 RowVersion setzen  
3.3 Die restlichen Properties definieren (Required, MaxLength usw.)  
3.4 DbSet f�r das Entity definieren  
3.5 DbSet in GetDbSet() eintragen  
  
4. Kontroller f�r das Entity erzeugen  
4.1 Abgeleitet vom GenericPersistenceController  
  
5. Das Entity auf die Datenbank �bertragen  
5.1 Migration starten und sprechenden Namen vergeben (add-migration Name)  
5.2 Migration auf die Datenbank �bertragen (update-database)  
  
6. Kontroller in die Factory-Klasse eintragen  
