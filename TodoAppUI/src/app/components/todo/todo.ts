import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, catchError, EMPTY } from 'rxjs';
import { TodoService } from '../../services/todo';
import { Todo, CreateTodoDto } from '../../models/todo.model';


@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo.html',
  styleUrls: ['./todo.css'],
})
export class TodoComponent implements OnInit {
  todos$ = new BehaviorSubject<Todo[]>([]);
  newTodo: string = '';
  editTitle: string = '';
  editingId: string | null = null;
  addingTodo: boolean = false;
  deletingTodoId: string | null = null;
  errorMessage: string = '';

  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    this.loadTodos();
  }

  // -------- LOAD ALL TODOS --------
  loadTodos() {
    this.todoService.getTodos()
      .pipe(
        catchError(err => {
          console.error('Failed to load todos', err);
          this.errorMessage = 'Failed to load todos';
          return EMPTY;
        })
      )
      .subscribe(todos => this.todos$.next(todos));
  }

  // -------- ADD NEW TODO --------
  addTodo() {
    const title = this.newTodo.trim();
    if (!title) return;

    this.addingTodo = true;
    const dto: CreateTodoDto = { title };

    this.todoService.addTodo(dto)
      .pipe(
        catchError(err => {
          console.error('AddTodo failed', err);
          this.errorMessage = 'Failed to add todo';
          this.addingTodo = false;
          return EMPTY;
        })
      )
      .subscribe(added => {
        const current = this.todos$.value;
        this.todos$.next([...current, added]);
        this.newTodo = '';
        this.addingTodo = false;
      });
  }

  // -------- DELETE TODO --------
  deleteTodo(id: string) {
    this.deletingTodoId = id;

    this.todoService.deleteTodo(id)
      .pipe(
        catchError(err => {
          console.error('DeleteTodo failed', err);
          this.errorMessage = 'Failed to delete todo';
          this.deletingTodoId = null;
          return EMPTY;
        })
      )
      .subscribe(() => {
        const current = this.todos$.value.filter(t => t.id !== id);
        this.todos$.next(current);
        this.deletingTodoId = null;
      });
  }

  // -------- TOGGLE COMPLETED --------
  toggle(todo: Todo) {
    this.todoService.toggleTodo(todo.id)
      .pipe(
        catchError(err => {
          console.error('Toggle failed', err);
          this.errorMessage = 'Failed to toggle todo';
          return EMPTY;
        })
      )
      .subscribe(updated => {
        const current = this.todos$.value.map(t => t.id === updated.id ? updated : t);
        this.todos$.next(current);
      });
  }

  // -------- EDITING --------
  startEdit(todo: Todo) {
    this.editingId = todo.id;
    this.editTitle = todo.title;
  }

  saveEdit(todo: Todo) {
    const title = this.editTitle.trim();
    if (!title) return;

    this.todoService.updateTodo(todo.id, title)
      .pipe(
        catchError(err => {
          console.error('Update failed', err);
          this.errorMessage = 'Failed to update todo';
          return EMPTY;
        })
      )
      .subscribe(updated => {
        const current = this.todos$.value.map(t => t.id === updated.id ? updated : t);
        this.todos$.next(current);
        this.cancelEdit();
      });
  }

  cancelEdit() {
    this.editingId = null;
    this.editTitle = '';
  }

  // -------- TRACKBY FOR NGFOR --------
  trackById(index: number, todo: Todo) {
    return todo.id;
  }
}
