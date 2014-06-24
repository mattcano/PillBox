class User < ActiveRecord::Base
  # Include default devise modules. Others available are:
  # :token_authenticatable, :confirmable,
  # :lockable, :timeoutable and :omniauthable
  devise :database_authenticatable, :registerable,
         :recoverable, :rememberable, :trackable, :validatable

  # Setup accessible (or protected) attributes for your model
  attr_accessible :email, :password, :password_confirmation, :remember_me, :name, :accepted_invitation, :phone_number, :phone_is_cell, :calls_enabled, :sms_enabled, :notification_freq, :email_enabled, :first_name, :last_name
  # attr_accessible :title, :body

  has_many :dependents_coaches, :class_name => "CoachesDependent", :foreign_key => :coach_id
  has_many :coaches_dependents, :class_name => "CoachesDependent", :foreign_key => :dependent_id
  has_many :dependents, :through => :dependents_coaches
  has_many :coaches, :through => :coaches_dependents

  has_many :medications
  has_many :reminders

  

end
