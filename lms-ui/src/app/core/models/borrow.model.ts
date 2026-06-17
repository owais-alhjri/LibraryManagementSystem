export interface BorrowRecord {
  id: string;
  userId: string;
  bookId: string;
  bookTitle: string;
  message: string;
  borrowedDate: string;
  returnedDate: string | null;
  status: string;
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

export interface ConfirmDialogData{
  title: string;
  message: string;
}