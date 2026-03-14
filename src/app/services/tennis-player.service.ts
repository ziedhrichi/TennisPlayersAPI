import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Player } from '../models/player.model'; // Adjust the import based on your project structure

@Injectable({
  providedIn: 'root'
})
export class TennisPlayerService {
  private apiUrl = 'https://api.yourservice.com/players'; // Replace with your actual API URL

  constructor(private http: HttpClient) { }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(this.apiUrl);
  }

  getPlayerById(id: number): Observable<Player> {
    return this.http.get<Player>(`${this.apiUrl}/${id}`);
  }

  createPlayer(player: Player): Observable<Player> {
    return this.http.post<Player>(this.apiUrl, player);
  }

  updatePlayer(id: number, player: Player): Observable<Player> {
    return this.http.put<Player>(`${this.apiUrl}/${id}`, player);
  }

  deletePlayer(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  searchPlayers(query: string): Observable<Player[]> {
    return this.http.get<Player[]>(`${this.apiUrl}?search=${query}`);
  }
}