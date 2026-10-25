1- Import Data Files

Solution minimaliste, extensible, sans package externe. Cible : .NET 8.

2- Exécuter

Depuis la racine du projet :
Lance les commandes suivantes : la première pour le fichier data.csv, et la seconde pour le fichier data.fixed.

```bash

dotnet run --project src/ImportCli -- csv datas/data.csv
dotnet run --project src/ImportCli -- fixed datas/data.fixed
dotnet test
```

Le résultat des deux commandes d’import produit le même tableau JSON.
Expected Output
Compact JSON: 
[{"OriginalSourceId":"F001","TransactionDate":"2026-10-10","Value":5050.89,"Rate":0.145},{"OriginalSourceId":"F003","TransactionDate":"2026-10-20","Value":12200.34,"Rate":1.25}]