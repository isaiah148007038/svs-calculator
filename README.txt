SVS Web App v3

Main upgrades:
- SQLite database at /data/players.db
- Hidden alliance/admin codes in appsettings.json
- Session-based login
- Admin-only delete/restore/export
- Soft delete instead of permanent delete
- Audit log for saves/deletes/restores/logins
- Screenshot upload path /data/uploads
- Render-ready Dockerfile

Default deploy notes:
1. In Render, add a Persistent Disk mounted to /data.
2. Set Root Directory blank.
3. Runtime: Docker.
4. No build/start commands.
5. Update appsettings.json codes before deploy.
