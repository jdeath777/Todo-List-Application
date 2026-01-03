import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Todo, CreateTodoDto } from '../models/todo.model';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  constructor(private http: HttpClient, private configService: ConfigService) {}

  private get baseUrl(): string {
    // Compose the runtime API base from the config service. ConfigService
    return `${this.configService.apiUrl.replace(/\/$/, '')}/Todo`;
  }

  // -------- GET ALL --------
  getTodos(): Observable<Todo[]> {
    return this.http.get<Todo[]>(this.baseUrl);
  }

  // -------- ADD --------
  addTodo(todo: CreateTodoDto): Observable<Todo> {
    return this.http.post<Todo>(this.baseUrl, todo);
  }

  // -------- DELETE --------
  deleteTodo(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

   // -------- UPDATE (EDIT) --------
  updateTodo(id: string, title: string): Observable<Todo> {
    return this.http.put<Todo>(`${this.baseUrl}/${id}`, { title });
  }

  // -------- TOGGLE --------
  toggleTodo(id: string): Observable<Todo> {
    return this.http.patch<Todo>(`${this.baseUrl}/${id}/toggle`, {});
  }

 
}
