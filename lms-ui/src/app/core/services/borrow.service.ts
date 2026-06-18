import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
  BorrowRecord,
  BorrowRequest,
  ReturnRequest,
  ReturnResponse,
} from '../models/borrow.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BorrowService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/borrow-records`;

  getMyBorrowRecords(): Observable<BorrowRecord[]> {
    return this.http.get<BorrowRecord[]>(`${this.apiUrl}/my`);
  }

  borrowBook(data: BorrowRequest): Observable<BorrowRecord> {
    return this.http.post<BorrowRecord>(this.apiUrl, data);
  }

returnBook(returnData: { bookId: string }): Observable<BorrowRecord> {
  return this.http.post<BorrowRecord>(`${this.apiUrl}/return`, returnData);
}
}
