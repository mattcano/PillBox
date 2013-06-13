class PillboxMailer < ActionMailer::Base
  default from: "pillbox.application@gmail.com"

  def welcome_email(user)
    @user = user
    @url = "http://pill-box.herokuapp.com/"
    mail(:to => user.email, :subject => "Welcome to PillBox")
  end
end
