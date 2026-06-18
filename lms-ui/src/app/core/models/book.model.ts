export interface Book{
  id: string;
  title: string;
  author: string;
  bookState: 'Available' | 'Borrowed' | 'Returned';
}

export interface PagedBooksResponse {
  items: Book[];
  totalCount: number;
  page: number;
  totalPages: number;
}

export interface CreateBookRequest {
  title: string;
  author: string;
}

export interface UpdateBookRequest {
  title?: string;
  author?: string;
  bookState?: string;
}
