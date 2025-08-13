import { Component, Input, Output, EventEmitter, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService, CustomerLocateRequest, Customer } from '../../services/customer.service';
import { finalize } from 'rxjs/operators';

interface FormData {
  accountNumber: string;
  ssn: string;
  birthdate: string;
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  phoneNumber: string;
}

interface FormErrors {
  accountNumber?: string;
  ssn?: string;
  birthdate?: string;
}

@Component({
  selector: 'app-customer-locate',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-locate.component.html',
  styleUrl: './customer-locate.component.css'
})
export class CustomerLocateComponent implements OnInit {
  @Input() formData!: FormData;
  @Output() onNext = new EventEmitter<Partial<FormData>>();
  @Output() onCustomerFound = new EventEmitter<Customer>();

  formState = signal({
    accountNumber: '',
    ssn: '',
    birthdate: '',
    _ssnDigits: '' // Store actual SSN digits for validation
  });

  errors = signal<FormErrors>({});
  isLoading = signal(false);
  apiError = signal<string>('');
  customerFound = signal<Customer | null>(null);

  constructor(private customerService: CustomerService) {}

  ngOnInit() {
    this.formState.set({
      accountNumber: this.formData?.accountNumber || '',
      ssn: this.formData?.ssn || '',
      birthdate: this.formData?.birthdate || '',
      _ssnDigits: ''
    });
  }

  handleInputChange(field: string, value: string) {
    let processedValue = value;
    
    // Format account number with spaces
    if (field === 'accountNumber') {
      // Remove all non-digits
      const digitsOnly = value.replace(/\D/g, '');
      // Limit to 16 digits
      const truncated = digitsOnly.slice(0, 16);
      // Add spaces every 4 digits
      processedValue = truncated.replace(/(\d{4})(?=\d)/g, '$1 ');
    }
    
    this.formState.update(prev => ({
      ...prev,
      [field]: processedValue
    }));
    
    // Clear error when user starts typing
    if (this.errors()[field as keyof FormErrors]) {
      this.errors.update(prev => ({
        ...prev,
        [field]: ''
      }));
    }
  }

  // Handle SSN input with masking
  handleSSNInput(value: string) {
    // Remove all non-digits
    const digitsOnly = value.replace(/\D/g, '');
    
    // Limit to 9 digits
    const truncated = digitsOnly.slice(0, 9);
    
    // Apply masking based on length
    let masked = '';
    if (truncated.length === 0) {
      masked = '';
    } else if (truncated.length <= 3) {
      // Show first 3 digits: 123
      masked = truncated;
    } else if (truncated.length <= 5) {
      // Show first 3 + dash + next 2: 123-45
      masked = truncated.substring(0, 3) + '-' + truncated.substring(3);
    } else {
      // Show full format: 123-45-6789
      masked = truncated.substring(0, 3) + '-' + truncated.substring(3, 5) + '-' + truncated.substring(5);
    }
    
    this.formState.update(prev => ({
      ...prev,
      ssn: masked,
      _ssnDigits: truncated // Store the actual digits for validation
    }));
    
    // Clear error when user starts typing
    if (this.errors().ssn) {
      this.errors.update(prev => ({
        ...prev,
        ssn: ''
      }));
    }
  }

  // Validate SSN format using actual digits
  validateSSN(ssnDigits: string): boolean {
    // Must be exactly 9 digits
    if (ssnDigits.length !== 9) {
      return false;
    }
    
    // Cannot be all same digits
    if (/^(\d)\1{8}$/.test(ssnDigits)) {
      return false;
    }
    
    // Cannot start with 000, 666, or 900-999
    const firstThree = parseInt(ssnDigits.substring(0, 3));
    if (firstThree === 0 || firstThree === 666 || (firstThree >= 900 && firstThree <= 999)) {
      return false;
    }
    
    // Cannot be 000000000
    if (ssnDigits === '000000000') {
      return false;
    }
    
    // Additional validation: middle digits cannot be 00
    const middleTwo = parseInt(ssnDigits.substring(3, 5));
    if (middleTwo === 0) {
      return false;
    }
    
    return true;
  }

  // Validate birthdate
  validateBirthdate(birthdate: string): boolean {
    if (!birthdate) {
      return false;
    }
    
    const date = new Date(birthdate);
    const today = new Date();
    
    // Check if it's a valid date
    if (isNaN(date.getTime())) {
      return false;
    }
    
    // Check if date is not in the future
    if (date > today) {
      return false;
    }
    
    // Check if person is at least 18 years old
    const age = today.getFullYear() - date.getFullYear();
    const monthDiff = today.getMonth() - date.getMonth();
    
    if (age < 18 || (age === 18 && monthDiff < 0) || (age === 18 && monthDiff === 0 && today.getDate() < date.getDate())) {
      return false;
    }
    
    return true;
  }

  validateForm(): boolean {
    const newErrors: FormErrors = {};
    
    // Account Number validation (16 digits)
    const accountDigits = this.formState().accountNumber.replace(/\s/g, '');
    if (!this.formState().accountNumber) {
      newErrors.accountNumber = 'Account number is required';
    } else if (!/^\d{16}$/.test(accountDigits)) {
      newErrors.accountNumber = 'Account number must be exactly 16 digits';
    }
    
    // SSN validation using actual digits
    if (!this.formState().ssn) {
      newErrors.ssn = 'SSN is required';
    } else if (!this.validateSSN(this.formState()._ssnDigits)) {
      newErrors.ssn = 'Please enter a valid SSN';
    }
    
    // Birthdate validation
    if (!this.formState().birthdate) {
      newErrors.birthdate = 'Birthdate is required';
    } else if (!this.validateBirthdate(this.formState().birthdate)) {
      newErrors.birthdate = 'You must be at least 18 years old and birthdate cannot be in the future';
    }
    
    this.errors.set(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.validateForm()) {
      this.isLoading.set(true);
      this.apiError.set('');

      const locateRequest: CustomerLocateRequest = {
        accountNumber: this.formState().accountNumber,
        ssn: this.formState()._ssnDigits, // Send actual digits, not masked
        birthdate: this.formState().birthdate
      };

      this.customerService.locateCustomer(locateRequest)
        .pipe(finalize(() => this.isLoading.set(false)))
        .subscribe({
          next: (customer) => {
            if (customer) {
              // Customer found - show success message and emit the found customer
              this.customerFound.set(customer);
              setTimeout(() => {
                this.onCustomerFound.emit(customer);
              }, 2000); // Show success message for 2 seconds
            } else {
              // Customer not found - proceed to next step (create new customer)
              this.onNext.emit(locateRequest);
            }
          },
          error: (error) => {
            console.error('Error locating customer:', error);
            this.apiError.set(error.message || 'Failed to locate customer. Please try again.');
          }
        });
    }
  }
}
