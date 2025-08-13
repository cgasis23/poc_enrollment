import { Component, Input, Output, EventEmitter, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface FormData {
  accountNumber: string;
  ssn: string;
  birthdate: string;
  fullName: string;
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  phoneNumber: string;
}

interface FormErrors {
  email?: string;
  username?: string;
  password?: string;
  confirmPassword?: string;
  phoneNumber?: string;
}

@Component({
  selector: 'app-setup-account',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './setup-account.component.html',
  styleUrl: './setup-account.component.css'
})
export class SetupAccountComponent implements OnInit {
  @Input() formData!: FormData;
  @Output() onNext = new EventEmitter<Partial<FormData>>();
  @Output() onBack = new EventEmitter<void>();

  formState = signal({
    email: '',
    username: '',
    password: '',
    confirmPassword: '',
    phoneNumber: ''
  });

  errors = signal<FormErrors>({});

  ngOnInit() {
    this.formState.set({
      email: this.formData?.email || '',
      username: this.formData?.username || '',
      password: this.formData?.password || '',
      confirmPassword: this.formData?.confirmPassword || '',
      phoneNumber: this.formData?.phoneNumber || ''
    });
  }

  handleInputChange(field: string, value: string) {
    this.formState.update(prev => ({
      ...prev,
      [field]: value
    }));
    
    // Clear error when user starts typing
    if (this.errors()[field as keyof FormErrors]) {
      this.errors.update(prev => ({
        ...prev,
        [field]: ''
      }));
    }
  }

  validateForm(): boolean {
    const newErrors: FormErrors = {};
    
    if (!this.formState().email) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(this.formState().email)) {
      newErrors.email = 'Please enter a valid email';
    }
    
    if (!this.formState().username) {
      newErrors.username = 'Username is required';
    } else if (this.formState().username.length < 3) {
      newErrors.username = 'Username must be at least 3 characters';
    }
    
    if (!this.formState().password) {
      newErrors.password = 'Password is required';
    } else if (this.formState().password.length < 6) {
      newErrors.password = 'Password must be at least 6 characters';
    }
    
    if (!this.formState().confirmPassword) {
      newErrors.confirmPassword = 'Please confirm your password';
    } else if (this.formState().password !== this.formState().confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }
    
    if (this.formState().phoneNumber && !/^\+?[\d\s\-\(\)]{10,}$/.test(this.formState().phoneNumber)) {
      newErrors.phoneNumber = 'Please enter a valid phone number';
    }
    
    this.errors.set(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.validateForm()) {
      this.onNext.emit(this.formState());
    }
  }

  handleBack() {
    this.onBack.emit();
  }
}
