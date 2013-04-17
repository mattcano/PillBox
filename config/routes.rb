PillBox::Application.routes.draw do

  devise_for :users

  root :to => 'pillbox#index'

end
