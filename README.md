# 🎾 Tennis Player API

Une API RESTful permettant de gérer et consulter des informations sur des joueurs de tennis.  
Le projet est conçu en **C# / .NET** avec une architecture claire (Model - Repository - Service - Controller) et des principes de sécurité intégrés.                       

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
- Tests unitaires
- Tests d'integration
- Utilisation JWT pour sécuriser les secrets
- Déploiement et intégration continue via GitHub Actions, Azure App Service et le pipline CI/CD
- Documentation avec Swagger
  
---

## 🧪 Stratégie de Tests

- **Unitaires** → test de la logique métier (Services, Repositories)  
- **Intégration** → validation des endpoints via un serveur de test .NET  
- **CI/CD** → tous les tests sont exécutés automatiquement avant chaque déploiement

---

## 📊 Observabilité & Logging

- **Logging** centralisé avec Serilog (console + fichiers + Azure Application Insights)
- **Monitoring** via Azure Monitor pour suivre les performances et erreurs
- Middleware de logging HTTP pour tracer chaque appel

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

## 🧰 Stack Technique

- **Langage & Framework** : C# / .NET 8
- **Sécurité** : JWT
- **Tests** : xUnit, Moq
- **CI/CD** : GitHub Actions, Azure App Service
- **Documentation** : Swagger / OpenAPI
- **Logs & Monitoring** : Serilog

---

## 📖 Documentation Swagger

Tu peux accéder à la documentation interactive Swagger ici : 

https://tennis-player-api-fqh6hhgjd7exegeu.francecentral-01.azurewebsites.net/

---

## 🛠️ Endpoints principaux

| Méthode | Endpoint                      | Description                     |
|---------|-------------------------------|---------------------------------|
| GET     | api/TennisPlayers             | Récupérer tous les joueurs      |
| GET     | api/TennisPlayers/{id}        | Récupérer un joueur par ID      |
| GET     | api/TennisPlayers/statistics  | Statiques sur les joueurs       |
| POST    | api/TennisPlayers             | Créer un nouveau joueur         |
| PUT     | api/TennisPlayers/{id}        | Mettre à jour un joueur existant|
| DELETE  | api/TennisPlayers/{id}        | Supprimer un joueur             |

---

## 🔐 Authentification & Rôles

L’API utilise **JSON Web Tokens (JWT)** pour sécuriser l’accès.  
Lors de la connexion (`/api/Auth/login`), un token est généré avec un rôle associé.

### Comptes de test disponibles

| Username | Password | Rôle   | Droits |
|----------|----------|--------|--------|
| `user`   | `1234`   | User 👤   | Lecture uniquement |
| `editor` | `1234`   | Editor 📝 | Lecture + Création + Modification |
| `admin`  | `1234`   | Admin 👑  | Lecture + Création + Modification + Suppression |

---

## 🗂️ Endpoints et rôles associés

| Endpoint                           | User 👤 | Editor 📝 | Admin 👑 |
|------------------------------------|---------|-----------|----------|
| `GET /api/TennisPlayers`           | ✅      | ✅        | ✅       |
| `GET /api/TennisPlayers/{id}`      | ✅      | ✅        | ✅       |
| `POST /api/TennisPlayers`          | ❌      | ✅        | ✅       |
| `PUT /api/TennisPlayers/{id}`      | ❌      | ✅        | ✅       |
| `DELETE /api/TennisPlayers/{id}`   | ❌      | ❌        | ✅       |
| `GET /api/TennisPlayers/statistics`| ✅      | ✅        | ✅       |

---

## ⚡ Tester l’API

Tu peux tester l’API avec :  

- **Swagger** : [Swagger](https://tennis-player-api-fqh6hhgjd7exegeu.francecentral-01.azurewebsites.net/)  
- **Authentification** : JWT Bearer Token  

-----

## 🔑 Gestion des rôles

L’API utilise un système de **rôles** pour restreindre l’accès :

- 👤 **User**
  - Peut consulter les joueurs (`GET`)
- 📝 **Editor**
  - Peut consulter (`GET`)
  - Peut créer et modifier (`POST`, `PUT`)
- 👑 **Admin**
  - A tous les droits (`GET`, `POST`, `PUT`, `DELETE`)

-----

### 🔑 Étapes pour tester avec JWT dans Swagger

1. **Obtenir un token**
   - Dans Swagger, appelle l’endpoint :
     ```
     POST /api/Auth/login
     ```
   - Exemple de body :
     ```json
     {
       "username": "admin",
       "password": "1234"
     }
     ```
   - Réponse :
     ```json
     {
       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
     }
     ```

2. **Configurer Swagger pour utiliser le token**
   - Clique sur le bouton **Authorize** (en haut à droite dans Swagger).  
   - Saisis le token sous la forme :
     ```
     Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
     ```
   - Valide.  

3. **Appeler les endpoints sécurisés**
   - Les endpoints comme :
     - `GET /api/TennisPlayers`
     - `POST /api/TennisPlayers`
     - `PUT /api/TennisPlayers/{id}`
     - `DELETE /api/TennisPlayers/{id}`
     
     nécessitent un utilisateur authentifié avec le rôle `admin`.  
   - Une fois le token ajouté, tu peux tester normalement.  

-----

### 📚 Exemple rapide avec `curl`

```bash
# Login pour obtenir un token
curl -X POST https://tennis-player-api-fqh6hhgjd7exegeu.francecentral-01.azurewebsites.net/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "1234"}'

# Utiliser le token pour accéder aux joueurs
curl -X GET https://tennis-player-api-fqh6hhgjd7exegeu.francecentral-01.azurewebsites.net/api/TennisPlayers \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6..."

```
---

## 🔮 Améliorations Futures

- Implémentation d’une base de données SQL (Azure SQL ou PostgreSQL)
- Implementer la securité avec Azure key vault
- Gestion des utilisateurs avec base de donnée pour les roles de la partie securité
- Mise en cache des statistiques avec Redis
- Ajout de tests de performance (ex : k6, JMeter)
- Documentation Postman collection exportée

---