import { Component, Input, Output, EventEmitter, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MFAService } from '../../services/mfa.service';

interface UserData {
  email: string;
  phoneNumber: string;
  username: string;
  accountNumber: string;
}

interface MFAErrors {
  general?: string;
}

interface MFAResult {
  mfaVerified: boolean;
  mfaMethod: string | null;
}

@Component({
  selector: 'app-mfa-setup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './mfa-setup.component.html',
  styleUrl: './mfa-setup.component.css'
})
export class MFASetupComponent {
  @Input() userData!: UserData;
  @Output() onComplete = new EventEmitter<MFAResult>();

  isEnrollmentStarted = signal(false);
  enrollmentId = signal<string>('');
  errors = signal<MFAErrors>({});
  isLoading = signal(false);

  constructor(private mfaService: MFAService) {}

  // Start Strivacity MFA enrollment
  async startMFAEnrollment() {
    this.isLoading.set(true);
    this.errors.set({});

    try {
      const response = await this.mfaService.completeEnrollmentAndSetupMFA({
        email: this.userData.email,
        phoneNumber: this.userData.phoneNumber,
        accountNumber: this.userData.accountNumber,
        userId: this.userData.username // Using username as userId for demo
      });

      if (response.success) {
        this.enrollmentId.set(response.enrollmentId || '');
        this.isEnrollmentStarted.set(true);
      } else {
        this.errors.set({ general: response.message });
      }
    } catch (error) {
      this.errors.set({ general: 'Failed to start MFA enrollment. Please try again.' });
    } finally {
      this.isLoading.set(false);
    }
  }

  // Send reminder email
  async sendReminder() {
    if (!this.enrollmentId()) return;

    this.isLoading.set(true);
    this.errors.set({});

    try {
      const response = await this.mfaService.sendMFAReminder(this.userData.email, this.enrollmentId());
      
      if (response.success) {
        // Show success message briefly
        setTimeout(() => {
          this.errors.set({});
        }, 3000);
      } else {
        this.errors.set({ general: response.message });
      }
    } catch (error) {
      this.errors.set({ general: 'Failed to send reminder. Please try again.' });
    } finally {
      this.isLoading.set(false);
    }
  }

  // Check MFA enrollment status
  async checkEnrollmentStatus() {
    if (!this.enrollmentId()) return;

    this.isLoading.set(true);
    this.errors.set({});

    try {
      const response = await this.mfaService.checkMFAEnrollmentStatus(this.enrollmentId());
      
      if (response.success && response.isEnrolled) {
        // MFA setup completed successfully
        this.onComplete.emit({ mfaVerified: true, mfaMethod: 'strivacity' });
      } else if (response.success) {
        this.errors.set({ general: 'MFA setup is still pending. Please check your email and complete the setup.' });
      } else {
        this.errors.set({ general: response.message });
      }
    } catch (error) {
      this.errors.set({ general: 'Failed to check MFA status. Please try again.' });
    } finally {
      this.isLoading.set(false);
    }
  }

  // Skip MFA setup
  handleSkipMFA() {
    this.onComplete.emit({ mfaVerified: false, mfaMethod: null });
  }

  // Retry enrollment
  retry() {
    this.errors.set({});
    if (this.isEnrollmentStarted()) {
      this.sendReminder();
    } else {
      this.startMFAEnrollment();
    }
  }

  // Go back to enrollment start
  goBack() {
    this.isEnrollmentStarted.set(false);
    this.enrollmentId.set('');
    this.errors.set({});
  }
}
