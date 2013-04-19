PillBox::Application.routes.draw do

  resources :reminders


  resources :medications


  devise_for :users

  root :to => 'home#index'
  match "/about" => "home#about"
  match "/credits" => "home#credits"

  match "/mypillbox" => "pillbox#mypillbox"
  match "/myreminders" => "pillbox#reminders"
  match "/meds_list" => "pillbox#meds_list"
  match "/coaches" => "pillbox#coaches"
  match "/buddies" => "pillbox#buddies"
  match "/settings" => "pillbox#settings"

  match 'user_root' => 'pillbox#mypillbox'

  # get "/medications/new", :controller => "medications", :action => "new"

end
