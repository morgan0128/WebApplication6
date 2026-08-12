import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface TodoItem {
  id: number;
  title: string;
  isComplete: boolean;
  createdAt: string;
}

@Component({
  selector: 'app-root',
  imports: [DatePipe, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/todos';

  protected readonly todos = signal<TodoItem[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly isSaving = signal(false);
  protected readonly error = signal<string | null>(null);
  protected newTodoTitle = '';

  protected readonly openCount = computed(() => this.todos().filter((todo) => !todo.isComplete).length);
  protected readonly completedCount = computed(() => this.todos().filter((todo) => todo.isComplete).length);

  ngOnInit(): void {
    this.loadTodos();
  }

  protected loadTodos(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.http.get<TodoItem[]>(this.apiUrl).subscribe({
      next: (todos) => {
        this.todos.set(todos);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Could not reach the Postgres-backed API. Check the connection string and make sure ASP.NET is running.');
        this.isLoading.set(false);
      }
    });
  }

  protected addTodo(): void {
    const title = this.newTodoTitle.trim();
    if (!title) {
      return;
    }

    this.isSaving.set(true);
    this.error.set(null);

    this.http.post<TodoItem>(this.apiUrl, { title }).subscribe({
      next: (todo) => {
        this.todos.update((todos) => [todo, ...todos]);
        this.newTodoTitle = '';
        this.isSaving.set(false);
      },
      error: () => {
        this.error.set('The item could not be saved.');
        this.isSaving.set(false);
      }
    });
  }

  protected toggleTodo(todo: TodoItem): void {
    this.http.put<TodoItem>(`${this.apiUrl}/${todo.id}`, { isComplete: !todo.isComplete }).subscribe({
      next: (updatedTodo) => {
        this.todos.update((todos) => todos.map((item) => item.id === updatedTodo.id ? updatedTodo : item));
      },
      error: () => this.error.set('The item could not be updated.')
    });
  }

  protected deleteTodo(todo: TodoItem): void {
    this.http.delete(`${this.apiUrl}/${todo.id}`).subscribe({
      next: () => {
        this.todos.update((todos) => todos.filter((item) => item.id !== todo.id));
      },
      error: () => this.error.set('The item could not be deleted.')
    });
  }
}
