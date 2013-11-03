class PillboxMailer < ActionMailer::Base
  default from: "Pillbox.Application@gmail.com"

  def welcome_email(user)
    @user = user
    mail(:to => @user.email, :subject => "Welcome to PillBox")
  end

  def reminder_email(user, reminder)
    @user = user
    @reminder = reminder
    @reminder_array = @user.reminders.order(:date).take(5)
    mail(:to => @user.email, :subject => "Pillbox Reminder Don't forget to #{@reminder.message}, #{@reminder.date.localtime.strftime("%m/%d/%y")}.!")
  end
end
