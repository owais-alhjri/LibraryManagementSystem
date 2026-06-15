import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Book, CreateBookRequest, PagedBooksResponse, UpdateBookRequest } from '../models/book.model';
import { D } from '@angular/cdk/keycodes';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Book`;

  getBooks(page: number, pageSize: number, search?: string):Observable<PagedBooksResponse>{
    let params = new HttpParams()
    .set('page', page)
    .set('pageSize', pageSize);

    if(search?.trim()){
      params = params.set('search', search.trim());
    }

    return this.http.get<PagedBooksResponse>(this.apiUrl, { params });
  }

  getBookById(id: string): Observable<Book>{
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  createBook(data: CreateBookRequest):Observable<Book>{
    return this.http.post<Book>(this.apiUrl, data);
  }

  updateBook(id: string, data: UpdateBookRequest): Observable<Book>{
    return this.http.patch<Book>(`${this.apiUrl}/${id}`, data);
  }

  deleteBook(id: string):Observable<void>{
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
