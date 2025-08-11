import { Component, Input, Output, EventEmitter, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  formState = signal({
    accountNumber: '',
    ssn: '',
    birthdate: ''
  });

  ngOnInit() {
    this.formState.set({
      accountNumber: this.formData?.accountNumber || '',
      ssn: this.formData?.ssn || '',
      birthdate: this.formData?.birthdate || ''
    });
  }

  handleInputChange(field: string, value: string) {
    this.formState.update(prev => ({
      ...prev,
      [field]: value
    }));
  }

  handleSubmit(event: Event) {
    event.preventDefault();
    this.onNext.emit(this.formState());
  }
}
