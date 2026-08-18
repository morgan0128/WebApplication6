// import { Component } from '@angular/core';
// import {DatePipe} from "@angular/common";
// import {FormsModule} from "@angular/forms";
import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {RouterOutlet} from '@angular/router';

interface TodoItem {
  id: number;
  title: string;
  isComplete: boolean;
  createdAt: string;
}

@Component({
  selector: 'app-work-queue',
    imports: [
        DatePipe,
        FormsModule
    ],
  templateUrl: './work-queue.html',
  styleUrl: './work-queue.css',
})
export class WorkQueue implements OnInit {

  private readonly http = inject(HttpClient);

  private readonly apiTodoUrl = '/api/todos';

  protected readonly todos = signal<TodoItem[]>([]);
  protected readonly todosIsLoading = signal(true);
  protected readonly todosIsSaving = signal(false);
  protected readonly todoError = signal<string | null>(null);
  protected newTodoTitle = '';

  protected readonly todosOpenCount = computed(() => this.todos().filter((todo) => !todo.isComplete).length);
  protected readonly todosCompletedCount = computed(() => this.todos().filter((todo) => todo.isComplete).length);

  ngOnInit(): void {
    this.loadTodos();
    // this.loadImages();
  }

  protected loadTodos(): void {
    this.todosIsLoading.set(true);
    this.todoError.set(null);

    this.http.get<TodoItem[]>(this.apiTodoUrl).subscribe({
      next: (todos) => {
        this.todos.set(todos);
        this.todosIsLoading.set(false);
      },
      error: () => {
        this.todoError.set('Could not reach the Postgres-backed API. Check the connection string and make sure ASP.NET is running.');
        this.todosIsLoading.set(false);
      }
    });
  }

  protected addTodo(): void {
    const title = this.newTodoTitle.trim();
    if (!title) {
      return;
    }

    this.todosIsSaving.set(true);
    this.todoError.set(null);

    this.http.post<TodoItem>(this.apiTodoUrl, { title }).subscribe({
      next: (todo) => {
        this.todos.update((todos) => [todo, ...todos]);
        this.newTodoTitle = '';
        this.todosIsSaving.set(false);
      },
      error: () => {
        this.todoError.set('The item could not be saved.');
        this.todosIsSaving.set(false);
      }
    });
  }



  protected toggleTodo(todo: TodoItem): void {
    this.http.put<TodoItem>(`${this.apiTodoUrl}/${todo.id}`, { isComplete: !todo.isComplete }).subscribe({
      next: (updatedTodo) => {
        this.todos.update((todos) => todos.map((item) => item.id === updatedTodo.id ? updatedTodo : item));
      },
      error: () => this.todoError.set('The item could not be updated.')
    });
  }

  protected deleteTodo(todo: TodoItem): void {
    this.http.delete(`${this.apiTodoUrl}/${todo.id}`).subscribe({
      next: () => {
        this.todos.update((todos) => todos.filter((item) => item.id !== todo.id));
      },
      error: () => this.todoError.set('The item could not be deleted.')
    });
  }

}


