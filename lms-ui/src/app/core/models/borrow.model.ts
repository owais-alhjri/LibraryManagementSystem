export interface BorrowRecord {
  id: string;
  userId: string;
  bookId: string;
  message: string;
  borrowedDate: string;
  returnedDate: string | null;
}

export interface BorrowRequest {
  bookId: string;
}

export interface ReturnRequest {
  bookId: string;
}

export interface ReturnResponse {
  id: string;
  returnedDate: string;
  message: string;
}
