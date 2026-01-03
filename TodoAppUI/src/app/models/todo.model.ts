export interface Todo {
  id: string;
  title: string;
  isCompleted?: boolean;
}

export interface CreateTodoDto {
  title: string;
}