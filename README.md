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

┣ 📂 Models # Définitions des entités (Player, Match, Ranking...)

┣ 📂 Repositories # Accès aux données (ex: PlayerRepository)

┣ 📂 Services # Logique métier (PlayerService)

┣ 📂 Controllers # Endpoints REST exposés (PlayerController)

┣ 📂 Security # Sécurité (JWT, Azure KeyVault, authentification)

┣ 📂 Config # Configuration & injection de dépendances

┗ 📂 Tests # Tests unitaires et d’intégration

---

## ✅ Bonnes pratiques

- Respecter le pattron de conception Repository - Service - Controller
- Appliquer le principe de SOLID
- Commenter le code source
- Séparation des responsabilités
- Tests unitaires pour la logique métier
- Utilisation d’Azure KeyVault pour sécuriser les secrets
- Documentation avec Swagger

