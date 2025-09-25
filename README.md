# 🎾 Tennis Player API

Une API RESTful permettant de gérer et consulter des informations sur des joueurs de tennis.  
Le projet est conçu en **C# / .NET** avec une architecture claire (Model - Repository - Service - Controller) et des principes de sécurité intégrés (KeyVault, gestion des secrets, authentification).                       

---

## 🚀 Fonctionnalités

- 📋 Consulter la liste des joueurs triée du meilleur au moins bon.
- 🔍 Rechercher un joueur par ID, prénom ou nom
- 📊 Retourner les statistiques suivantes :
  - Pays qui a le plus grand ratio de parties gagnées
  - IMC moyen de tous les joueurs
  - La médiane de la taille des joueurs 
- ➕ Ajouter un joueur
- ✏️ Mettre à jour les informations d’un joueur
- ❌ Supprimer un joueur
- 🔐 Sécurisation via Azure KeyVault et JWT

---

## 🏗️ Design & Architecture

L’API suit une architecture en couches :  

📂 TennisPlayerAPI

┣ 📂 Data : Source de donnée (Json file dans notre cas).

┣ 📂 Models : Définitions des entités.

┣ 📂 Repositories : Accès aux données.

┣ 📂 Services : Logique métier.

┣ 📂 Controllers : Endpoints REST exposés.

┣ 📂 Exceptions : Les Exceptions.

┣ 📂 Security : Sécurité (JWT dans notre cas)

┣ 📂 Config : Configuration & injection de dépendances

┗ 📂 Tests : Tests unitaires et d’intégration

---

## ✅ Bonnes pratiques

- Respecter le pattron de conception Repository
- Appliquer le principe de SOLID
- Commenter le code source
- Séparation des responsabilités
- Tests unitaires pour la logique métier
- Tests d'integration pour vérifier que les différentes couches (contrôleurs, services, repositories, base de données, authentification, etc.) fonctionnent correctement ensemble dans un scénario réel ou quasi-réel.
- Utilisation JWT pour sécuriser les secrets
- Déploiement et intégration continue via GitHub Actions, Azure App Service et le pipline CI/CD
- Documentation avec Swagger
  
---

## 🚀 Déploiement & Intégration Continue

Ce projet est entièrement automatisé via GitHub Actions et Azure App Service.
Le pipeline CI/CD exécute plusieurs étapes clés pour garantir la qualité et la fiabilité du déploiement :

- Récupération du code → téléchargement automatique du code source depuis GitHub.
- Configuration de l’environnement .NET → installation du SDK .NET Core nécessaire.
- Compilation → construction du projet avec dotnet build.
- Tests unitaires et integration → exécution automatique des tests avec dotnet test.
  Si un test échoue, le déploiement est bloqué.
- Publication → génération d’un package optimisé via dotnet publish.
- Création d’artefacts → préparation des fichiers publiés pour le déploiement.
- Déploiement Azure → livraison de l’API directement sur Azure App Service.

Grâce à ce pipeline, chaque commit sur la branche principale est testé, validé et déployé automatiquement

---

## 📖 Documentation Swagger

Tu peux accéder à la documentation interactive Swagger ici :  
https://tennis-player-api-fqh6hhgjd7exegeu.francecentral-01.azurewebsites.net/

---

## 🛠️ Endpoints principaux

| Méthode | Endpoint                   | Description                     |
|---------|----------------------------|---------------------------------|
| GET     | /TennisPlayers             | Récupérer tous les joueurs      |
| GET     | /TennisPlayers/{id}        | Récupérer un joueur par ID      |
| GET     | /TennisPlayers/statistics  | Statiques sur les joueurs       |
| POST    | /TennisPlayers             | Créer un nouveau joueur         |
| PUT     | /TennisPlayers/{id}        | Mettre à jour un joueur existant|
| DELETE  | /TennisPlayers/{id}        | Supprimer un joueur             |

---

## ⚡ Tester l’API

Tu peux tester l’API avec :  
- **Swagger UI** (recommandé) 
- **Postman** ou **curl** :
```bash
curl https://tennis-player-api-fqh6hhgjd7exegeu.francecentral-01.azurewebsites.net/TennisPlayers

