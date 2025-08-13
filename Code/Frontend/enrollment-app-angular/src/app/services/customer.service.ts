import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  accountNumber?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  dateOfBirth: string;
  ssn?: string;
  status: string;
  createdAt: string;
  updatedAt?: string;
  isMfaEnabled: boolean;
  mfaEnabledAt?: string;
}

export interface CustomerCreate {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  accountNumber?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  dateOfBirth: string;
  ssn?: string;
}

export interface CustomerSearch {
  firstName?: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  status?: string;
  createdFrom?: string;
  createdTo?: string;
  page?: number;
  pageSize?: number;
}

export interface CustomerLocateRequest {
  accountNumber: string;
  ssn: string;
  birthdate: string;
}

export interface CustomerSearchResponse {
  customers: Customer[];
  totalCount: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly baseUrl = 'http://localhost:5065/api/customers'; // Adjust port as needed

  constructor(private http: HttpClient) { }

  /**
   * Search for customers with optional filters
   */
  searchCustomers(searchParams: CustomerSearch): Observable<CustomerSearchResponse> {
    let params = new HttpParams();
    
    if (searchParams.firstName) params = params.set('firstName', searchParams.firstName);
    if (searchParams.lastName) params = params.set('lastName', searchParams.lastName);
    if (searchParams.email) params = params.set('email', searchParams.email);
    if (searchParams.phoneNumber) params = params.set('phoneNumber', searchParams.phoneNumber);
    if (searchParams.status) params = params.set('status', searchParams.status);
    if (searchParams.createdFrom) params = params.set('createdFrom', searchParams.createdFrom);
    if (searchParams.createdTo) params = params.set('createdTo', searchParams.createdTo);
    if (searchParams.page) params = params.set('page', searchParams.page.toString());
    if (searchParams.pageSize) params = params.set('pageSize', searchParams.pageSize.toString());

    return this.http.get<Customer[]>(this.baseUrl, { params, observe: 'response' })
      .pipe(
        map(response => {
          const totalCount = parseInt(response.headers.get('X-Total-Count') || '0');
          const page = parseInt(response.headers.get('X-Page') || '1');
          const pageSize = parseInt(response.headers.get('X-PageSize') || '10');
          
          return {
            customers: response.body || [],
            totalCount,
            page,
            pageSize
          };
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Get customer by ID
   */
  getCustomerById(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  /**
   * Get customer by email
   */
  getCustomerByEmail(email: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/email/${encodeURIComponent(email)}`)
      .pipe(catchError(this.handleError));
  }

  /**
   * Create a new customer
   */
  createCustomer(customer: CustomerCreate): Observable<Customer> {
    return this.http.post<Customer>(this.baseUrl, customer)
      .pipe(catchError(this.handleError));
  }

  /**
   * Update an existing customer
   */
  updateCustomer(id: number, customer: Partial<CustomerCreate>): Observable<Customer> {
    return this.http.put<Customer>(`${this.baseUrl}/${id}`, customer)
      .pipe(catchError(this.handleError));
  }

  /**
   * Delete a customer
   */
  deleteCustomer(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  /**
   * Check if customer exists
   */
  customerExists(id: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}/${id}/exists`)
      .pipe(catchError(this.handleError));
  }

  /**
   * Get customer statistics
   */
  getCustomerStats(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/stats`)
      .pipe(catchError(this.handleError));
  }

  /**
   * Locate customer by account number, SSN, and birthdate
   */
  locateCustomer(locateRequest: CustomerLocateRequest): Observable<Customer | null> {
    const params = new HttpParams()
      .set('accountNumber', locateRequest.accountNumber.replace(/\s/g, ''))
      .set('ssn', locateRequest.ssn)
      .set('birthdate', locateRequest.birthdate);

    return this.http.get<Customer>(`${this.baseUrl}/locate`, { params })
      .pipe(
        map(customer => customer),
        catchError(error => {
          if (error.status === 404) {
            // Customer not found - return null instead of throwing error
            return [null];
          }
          return this.handleError(error);
        })
      );
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: any): Observable<never> {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = error.error?.error || error.message || `Error Code: ${error.status}`;
    }
    
    console.error('CustomerService error:', error);
    return throwError(() => new Error(errorMessage));
  }
}
