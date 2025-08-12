import { Component, Input, Output, EventEmitter, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MFAService } from '../../services/mfa.service';

interface UserEnrollmentData {
  email: string;
  phoneNumber?: string;
  accountNumber: string;
  userId: string;
}

@Component({
  selector: 'app-strivacity-integration',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card w-full max-w-md">
      <div class="text-center mb-6">
        <h2 class="text-2xl font-bold text-gray-900 mb-2">MFA Setup</h2>
        <p class="text-gray-600">Strivacity Integration</p>
      </div>

      <!-- Step 1: Enrollment Completion -->
      <div *ngIf="currentStep() === 'enrollment'" class="space-y-4">
        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
          <h3 class="font-semibold text-blue-900 mb-2">Complete Your Enrollment</h3>
          <p class="text-blue-700 text-sm">
            Your account has been created successfully. To complete your enrollment, 
            you'll need to set up Multi-Factor Authentication (MFA) through our 
            secure partner, Strivacity.
          </p>
        </div>

        <div class="space-y-3">
          <div class="flex items-center space-x-3">
            <div class="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
              <span class="text-green-600 text-sm font-semibold">✓</span>
            </div>
            <span class="text-sm text-gray-700">Account verification completed</span>
          </div>
          <div class="flex items-center space-x-3">
            <div class="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
              <span class="text-green-600 text-sm font-semibold">✓</span>
            </div>
            <span class="text-sm text-gray-700">Credentials created successfully</span>
          </div>
          <div class="flex items-center space-x-3">
            <div class="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
              <span class="text-blue-600 text-sm font-semibold">3</span>
            </div>
            <span class="text-sm text-gray-700">MFA setup with Strivacity</span>
          </div>
        </div>

        <button 
          (click)="completeEnrollment()"
          [disabled]="isLoading()"
          class="btn-primary w-full"
        >
          <span *ngIf="isLoading()" class="flex items-center justify-center">
            <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            Completing Enrollment...
          </span>
          <span *ngIf="!isLoading()">Complete Enrollment & Setup MFA</span>
        </button>
      </div>

      <!-- Step 2: Strivacity Link Sent -->
      <div *ngIf="currentStep() === 'link-sent'" class="space-y-4">
        <div class="bg-green-50 border border-green-200 rounded-lg p-4">
          <h3 class="font-semibold text-green-900 mb-2">Check Your Email</h3>
          <p class="text-green-700 text-sm">
            We've sent you an email with a secure link to complete your MFA setup 
            through Strivacity. Please check your inbox and follow the instructions.
          </p>
        </div>

        <div class="bg-gray-50 border border-gray-200 rounded-lg p-4">
          <h4 class="font-medium text-gray-900 mb-2">What to expect:</h4>
          <ul class="text-sm text-gray-700 space-y-1">
            <li>• Email from Strivacity with secure enrollment link</li>
            <li>• Dedicated Strivacity portal for MFA setup</li>
            <li>• Multiple MFA options (SMS, authenticator app, etc.)</li>
            <li>• Enterprise-grade security and compliance</li>
          </ul>
        </div>

        <div class="space-y-3">
          <button 
            (click)="sendReminder()"
            [disabled]="isLoading()"
            class="btn-secondary w-full"
          >
            <span *ngIf="isLoading()">Sending Reminder...</span>
            <span *ngIf="!isLoading()">Resend Email</span>
          </button>
          
          <button 
            (click)="checkStatus()"
            [disabled]="isLoading()"
            class="btn-outline w-full"
          >
            <span *ngIf="isLoading()">Checking Status...</span>
            <span *ngIf="!isLoading()">Check MFA Status</span>
          </button>
        </div>
      </div>

      <!-- Step 3: MFA Completed -->
      <div *ngIf="currentStep() === 'completed'" class="space-y-4">
        <div class="bg-green-50 border border-green-200 rounded-lg p-4">
          <h3 class="font-semibold text-green-900 mb-2">MFA Setup Complete!</h3>
          <p class="text-green-700 text-sm">
            Congratulations! Your Multi-Factor Authentication has been successfully 
            configured through Strivacity. Your account is now fully secured.
          </p>
        </div>

        <div class="space-y-3">
          <div class="flex items-center space-x-3">
            <div class="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
              <span class="text-green-600 text-sm font-semibold">✓</span>
            </div>
            <span class="text-sm text-gray-700">Account verification completed</span>
          </div>
          <div class="flex items-center space-x-3">
            <div class="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
              <span class="text-green-600 text-sm font-semibold">✓</span>
            </div>
            <span class="text-sm text-gray-700">Credentials created successfully</span>
          </div>
          <div class="flex items-center space-x-3">
            <div class="w-8 h-8 bg-green-100 rounded-full flex items-center justify-center">
              <span class="text-green-600 text-sm font-semibold">✓</span>
            </div>
            <span class="text-sm text-gray-700">MFA setup with Strivacity completed</span>
          </div>
        </div>

        <button 
          (click)="onComplete.emit()"
          class="btn-primary w-full"
        >
          Continue to Dashboard
        </button>
      </div>

      <!-- Error State -->
      <div *ngIf="error()" class="bg-red-50 border border-red-200 rounded-lg p-4">
        <h3 class="font-semibold text-red-900 mb-2">Error</h3>
        <p class="text-red-700 text-sm">{{ error() }}</p>
        <button 
          (click)="retry()"
          class="mt-3 btn-secondary text-sm"
        >
          Try Again
        </button>
      </div>
    </div>
  `,
  styles: [`
    .btn-primary {
      @apply bg-blue-600 text-white px-4 py-2 rounded-lg font-medium hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed;
    }
    
    .btn-secondary {
      @apply bg-gray-600 text-white px-4 py-2 rounded-lg font-medium hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed;
    }
    
    .btn-outline {
      @apply border border-gray-300 text-gray-700 px-4 py-2 rounded-lg font-medium hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed;
    }
  `]
})
export class StrivacityIntegrationComponent implements OnInit {
  @Input() userData!: UserEnrollmentData;
  @Output() onComplete = new EventEmitter<void>();

  currentStep = signal<'enrollment' | 'link-sent' | 'completed'>('enrollment');
  isLoading = signal(false);
  error = signal<string>('');
  enrollmentId = signal<string>('');

  constructor(private mfaService: MFAService) {}

  ngOnInit() {
    // Check if user already has an enrollment in progress
    this.checkExistingEnrollment();
  }

  async completeEnrollment() {
    this.isLoading.set(true);
    this.error.set('');

    try {
      const response = await this.mfaService.completeEnrollmentAndSetupMFA(this.userData);
      
      if (response.success) {
        this.enrollmentId.set(response.enrollmentId || '');
        this.currentStep.set('link-sent');
      } else {
        this.error.set(response.message);
      }
    } catch (err) {
      this.error.set('An error occurred while completing your enrollment. Please try again.');
    } finally {
      this.isLoading.set(false);
    }
  }

  async sendReminder() {
    if (!this.enrollmentId()) return;

    this.isLoading.set(true);
    this.error.set('');

    try {
      const response = await this.mfaService.sendMFAReminder(this.userData.email, this.enrollmentId());
      
      if (response.success) {
        // Show success message
        setTimeout(() => {
          this.error.set('');
        }, 3000);
      } else {
        this.error.set(response.message);
      }
    } catch (err) {
      this.error.set('Failed to send reminder. Please try again.');
    } finally {
      this.isLoading.set(false);
    }
  }

  async checkStatus() {
    if (!this.enrollmentId()) return;

    this.isLoading.set(true);
    this.error.set('');

    try {
      const response = await this.mfaService.checkMFAEnrollmentStatus(this.enrollmentId());
      
      if (response.success && response.isEnrolled) {
        this.currentStep.set('completed');
      } else if (response.success) {
        this.error.set('MFA setup is still pending. Please check your email and complete the setup.');
      } else {
        this.error.set(response.message);
      }
    } catch (err) {
      this.error.set('Failed to check MFA status. Please try again.');
    } finally {
      this.isLoading.set(false);
    }
  }

  private async checkExistingEnrollment() {
    // In a real implementation, you would check if the user already has an enrollment
    // This is a simplified version
    if (this.enrollmentId()) {
      this.currentStep.set('link-sent');
    }
  }

  retry() {
    this.error.set('');
    if (this.currentStep() === 'enrollment') {
      this.completeEnrollment();
    } else if (this.currentStep() === 'link-sent') {
      this.sendReminder();
    }
  }
}
