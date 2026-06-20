import { HttpErrorResponse } from '@angular/common/http';

interface ProblemDetails {
  title?: string;
  detail?: string;
}

export function extractErrorMessage(error: HttpErrorResponse): string {
  const problem = error.error as ProblemDetails | null;

  if (problem?.detail) return problem.detail;

  switch (error.status) {
    case 0:
      return 'Unable to reach the server. Check your connection.';
    case 400:
      return problem?.title ?? 'Invalid request.';
    case 401:
      return 'Your session has expired. Please log in again.';
    case 403:
      return "You don't have permission to do this.";
    case 404:
      return 'Resource not found.';
    case 409:
      return problem?.detail ?? 'This conflicts with existing data.';
    case 500:
      return 'Something went wrong on our end. Please try again.';
    default:
      return 'An unexpected error occurred.';
  }
}
